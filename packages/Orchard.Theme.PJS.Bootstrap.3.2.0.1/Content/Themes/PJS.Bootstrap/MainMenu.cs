﻿using System;
using System.Linq;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Navigation.Models;
using Orchard.Localization;
using Orchard.UI.Navigation;
using Orchard.Core.Title.Models;

namespace PJS.ReTouch {
    public class MainMenu : IMenuProvider {
        private readonly IContentManager _contentManager;
        private readonly IOrchardServices _orchardServices;

        public MainMenu(IContentManager contentManager, IOrchardServices orchardServices) {
            _contentManager = contentManager;
            _orchardServices = orchardServices;
        }

        public Localizer T { get; set; }
        public void GetMenu(IContent menu, NavigationBuilder builder) {
            if (menu.As<TitlePart>().Title == "Main Menu") {
                var menuParts = _contentManager
                    .Query<MenuPart, MenuPartRecord>()
                    .Where(x => x.MenuId == menu.Id)
                    .OrderBy(x => x.MenuPosition)
                    .List();

                var itemCount = Convert.ToInt32(decimal.Parse(menuParts.Last().MenuPosition)) + 1;

                if (_orchardServices.WorkContext.CurrentUser != null) {
                    builder.Add(T(_orchardServices.WorkContext.CurrentUser.UserName), itemCount.ToString(), item => item.Url("#").AddClass("menuUserName"));
                    builder.Add(T("Change Password"), itemCount.ToString() + ".1", item => item.Action("ChangePassword", "Account", new { area = "Orchard.Users" }));
                    builder.Add(T("Sign Out"), itemCount.ToString() + ".2", item => item.Action("LogOff", "Account", new { area = "Orchard.Users" }));
                    if (_orchardServices.Authorizer.Authorize(Orchard.Security.StandardPermissions.AccessAdminPanel)) {
                        builder.Add(T("Dashboard"), itemCount.ToString() + ".3", item => item.Action("Index", "Admin", new { area = "Dashboard" }));
                    }
                }
                else {
                    builder.Add(T("Sign In"), itemCount.ToString(), item => item.Action("LogOn", "Account", new { area = "Orchard.Users" }));
                }
            }
        }

        public string MenuName { get { return "Main Menu"; } }
    }
}