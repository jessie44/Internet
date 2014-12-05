using PJS.Bootstrap.Models;
using Orchard;

namespace PJS.Bootstrap.Services {
    public interface IThemeSettingsService : IDependency {
        ThemeSettingsRecord GetSettings();
    }
}