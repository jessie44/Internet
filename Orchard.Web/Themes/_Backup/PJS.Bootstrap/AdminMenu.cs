using Orchard.Localization;
using Orchard.Themes.Services;
using Orchard.UI.Navigation;

namespace PJS.Bootstrap {
    public class AdminMenu : INavigationProvider {
        private readonly ISiteThemeService _siteThemeService;

        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public AdminMenu(ISiteThemeService siteThemeService) {
            _siteThemeService = siteThemeService;
        }

        public void GetNavigation(NavigationBuilder builder) {
            if (IsBootstrapThemeOrBasedOnBootstrapTheme()) {
                builder.AddImageSet("themes")
                    .Add(T("Themes"), "10", BuildMenu);
            }
        }

        private void BuildMenu(NavigationItemBuilder menu) {
            menu.Add(T(""), "10.0",
                item => item
                    .Action("Index", "Admin", new { area = "Orchard.Themes" })
                    .Permission(Orchard.Themes.Permissions.ApplyTheme)
            );
            menu.Add(T("MBM Layout Options"), "10.1",
                item => item
                    .Action("Index", "Admin", new { area = Constants.RoutesAreaName })
                    .Permission(Permissions.ManageThemeSettings)
            );
        }

        private bool IsBootstrapThemeOrBasedOnBootstrapTheme() {
            var theme = _siteThemeService.GetSiteTheme();
            if (theme.Name == Constants.ThemeName || theme.BaseTheme == Constants.ThemeName)
                return true;
            if (theme.Name.Contains(Constants.ThemeName)) //Should find a way I could recursivly check if Bootstrap is the base theme.
                return true;
            return false;
        }
    }
}