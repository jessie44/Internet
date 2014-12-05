using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Orchard;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc;
using Orchard.Roles.Models;
using Orchard.Roles.Services;
using Orchard.Security;
using Orchard.Security.Providers;
using Orchard.UI.Notify;
using Orchard.Users.Events;

namespace AlexZh.WindowsAuthentication.Services
{
	[OrchardSuppressDependency("Orchard.Security.Providers.FormsAuthenticationService")]
	public class CustomAuthenticationService : IAuthenticationService
    {
	    private static readonly Type[] systemAuthenticationServiceParameterTypes;
	    private static readonly Func<WorkContext, Type, object> serviceResolver;

	    private readonly IWorkContextAccessor workContextAccessor;
		private IAuthenticationService systemAuthenticationService;
		private IMembershipService membershipService;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly IRoleService roleService;
		private readonly IRepository<UserRolesPartRecord> userRolesRepository;
		private readonly IWinAuthSettingsService winUserSettingsService;
		private readonly IOrchardServices orchardServices;
		private readonly IEnumerable<IUserEventHandler> userEventHandlers;

		public ILogger Logger { get; set; }
		public Localizer T { get; set; }
        public TimeSpan ExpirationTimeSpan { get; set; }

	    static CustomAuthenticationService()
        {
	        var typeInfo = typeof (FormsAuthenticationService);
	        var constructor = typeInfo.GetConstructors().FirstOrDefault();
            if (constructor == null)
                throw new InvalidOperationException("Unable to find constructor for the system authentication service");
	        var parameters = constructor.GetParameters();
	        systemAuthenticationServiceParameterTypes = parameters.Select(pi => pi.ParameterType).ToArray();

	        typeInfo = typeof(WorkContext);
	        var resolveMethod = typeInfo.GetMethods().First(mi => mi.IsGenericMethod && mi.GetParameters().Length == 0 && mi.Name.Equals("Resolve", StringComparison.Ordinal));
	        serviceResolver = (c, t) => {
	            var gmi = resolveMethod.MakeGenericMethod(t);
	            return gmi.Invoke(c, null);
	        };
	    }

		public CustomAuthenticationService(
            IWorkContextAccessor workContextAccessor,
			//IMembershipService membershipService,
			IRoleService roleService,
			IHttpContextAccessor httpContextAccessor,
			IRepository<UserRolesPartRecord> userRolesRepository,
			IWinAuthSettingsService winUserSettingsService,
			IEnumerable<IUserEventHandler> userEventHandlers
            )
        {
            this.workContextAccessor = workContextAccessor;
            this.roleService = roleService;
		    this.httpContextAccessor = httpContextAccessor;
            this.userRolesRepository = userRolesRepository;
            this.winUserSettingsService = winUserSettingsService;
			this.userEventHandlers = userEventHandlers;

			Logger = NullLogger.Instance;
			T = NullLocalizer.Instance;
            ExpirationTimeSpan = TimeSpan.FromDays(30);
		}

        #region Autofac

	    public IAuthenticationService GetSystemAuthenticationService()
        {
	        var context = workContextAccessor.GetContext();
            if (context == null)
                throw new InvalidOperationException("Unable to resolve work context");

	        var parameterValues = systemAuthenticationServiceParameterTypes.Select(t => serviceResolver(context, t));
	        var service = (FormsAuthenticationService) Activator.CreateInstance(typeof (FormsAuthenticationService), parameterValues.ToArray());
	        service.Logger = Logger;
	        service.ExpirationTimeSpan = ExpirationTimeSpan;
	        return service;
	    }

	    public IMembershipService GetMembershipService()
        {
            var context = workContextAccessor.GetContext();
            if (context == null)
                throw new InvalidOperationException("Unable to resolve work context");
	        return context.Resolve<IMembershipService>();
	    }

        #endregion

        #region UserName from WindowsIdentity

        private static string GetUserName(IIdentity identity)
		{
			return identity == null ? null : GetUserName(identity.Name);
		}

		private static string GetUserName(string identityName)
		{
			if (string.IsNullOrEmpty(identityName))
				return identityName;
			int index = identityName.IndexOf('\\');
			if (index > 0 && index < (identityName.Length - 1))
			{
				identityName = identityName.Substring(index + 1);
			}
			index = identityName.IndexOf('@');
			if (index > 0 && index < (identityName.Length - 1))
			{
				identityName = identityName.Remove(index);
			}

			return identityName;
		}

		#endregion

		#region Implementation of IAuthenticationService

		public void SignIn(IUser user, bool createPersistentCookie)
		{
			systemAuthenticationService.SignIn(user, createPersistentCookie);
		}

		public void SignOut()
		{
			systemAuthenticationService.SignOut();
		}

		public void SetAuthenticatedUserForRequest(IUser user)
		{
			systemAuthenticationService.SetAuthenticatedUserForRequest(user);
		}

		public IUser GetAuthenticatedUser() {
		    if (systemAuthenticationService == null)
		        systemAuthenticationService = GetSystemAuthenticationService();

			// Try to use default authentication seervice
			var user = systemAuthenticationService.GetAuthenticatedUser();
			if (user == null)
			{
				var httpContext = httpContextAccessor.Current();

				if (httpContext != null && httpContext.Request.IsAuthenticated && (httpContext.User.Identity is WindowsIdentity))
				{
					// Windows authentication
					var settings = winUserSettingsService.Retrieve();
					if (!settings.EnableWindowsAuthentication)
					{
						orchardServices.Notifier.Add(NotifyType.Error, T("Windows authentication is disabled. Please contact administrator."));
						return null;
					}

					var windowsIdentity = (WindowsIdentity) httpContext.User.Identity;
					string userName = GetUserName(windowsIdentity);
				    if (membershipService == null)
				        membershipService = GetMembershipService();
					user = membershipService.GetUser(userName);

					// Creating new user if not exists
					if (user == null)
					{
						string email = settings.EmailDomain;
						if (string.IsNullOrEmpty(email))
							email = null;
						else
							email = userName + "@" + email;

						user = membershipService.CreateUser(new CreateUserParams(
						                                    	userName,
						                                    	Guid.NewGuid().ToString(),
						                                    	email,
						                                    	Guid.NewGuid().ToString(),
						                                    	Guid.NewGuid().ToString(),
						                                    	true));
						// Adding default roles to user
						if (user != null)
						{
							var roles = settings.DefaultRoles.ToArray();

							foreach (var role in roles)
							{
								var roleRecord = roleService.GetRoleByName(role);
								if (roleRecord != null)
									userRolesRepository.Create(new UserRolesPartRecord {Role = roleRecord, UserId = user.Id});
							}
						}
					}
					// Signing In Windows User
					if (user != null)
					{
						SetAuthenticatedUserForRequest(user);
						SignIn(user, false);
						foreach (var userEventHandler in userEventHandlers)
						{
							userEventHandler.LoggedIn(user);
						}
					}
				}
			}

			return user;
		}

		#endregion
	}
}