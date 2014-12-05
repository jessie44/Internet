using System.Collections.Generic;
using System.Linq;
using AlexZh.WindowsAuthentication.Models;
using AlexZh.WindowsAuthentication.ViewModels;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Roles.Services;

namespace AlexZh.WindowsAuthentication.Drivers
{
	public class WinAuthSettingsPartDriver : ContentPartDriver<WinAuthSettingsPart>
	{
		private readonly IRoleService roleService;
		private readonly ISignals signals;

		public WinAuthSettingsPartDriver(ISignals signals, IRoleService roleService)
		{
			this.signals = signals;
			this.roleService = roleService;
		}

		protected override string Prefix
		{
			get { return "WinAuthSettings"; }
		}

		// GET
		protected override DriverResult Editor(WinAuthSettingsPart part, dynamic shapeHelper)
		{
			IEnumerable<RoleEntry> roles = roleService.GetRoles().Select(x => new RoleEntry
			                                                                   	{
			                                                                   		RoleId = x.Id,
			                                                                   		Name = x.Name,
			                                                                   		Selected = part.DefaultRoles.Contains(x.Name)
			                                                                   	});
			var model = new WinAuthSettingsPartViewModel
			            	{
			            		EnableWindowsAuthentication = true,
								EmailDomain = "mbmfoodservice.com",
			            		Roles = roles.ToList(),
			            	};
			return ContentShape("Parts_WinAuthSettings_Edit",
			                    () =>
			                    shapeHelper.EditorTemplate(TemplateName: "Parts/WinAuthSettings", Model: model, Prefix: Prefix)).
				OnGroup("users");
		}

		// POST
		protected override DriverResult Editor(WinAuthSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
		{
			var model = new WinAuthSettingsPartViewModel();
			if (updater.TryUpdateModel(model, Prefix, null, null))
			{
				signals.Trigger("AlexZh.WindowsAuthentication.SettingsChanged");
				part.EnableWindowsAuthentication = true;
				part.EmailDomain = "mbmfoodservice.com";
				part.DefaultRoles = model.Roles.Where(r => r.Selected).Select(r => r.Name);
			}
			return ContentShape("Parts_WinAuthSettings_Edit",
			                    () =>
			                    shapeHelper.EditorTemplate(TemplateName: "Parts/WinAuthSettings", Model: model, Prefix: Prefix)).
				OnGroup("users");
		}
	}
}