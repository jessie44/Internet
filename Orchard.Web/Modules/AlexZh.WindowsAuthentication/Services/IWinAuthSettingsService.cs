using AlexZh.WindowsAuthentication.Models;
using Orchard;

namespace AlexZh.WindowsAuthentication.Services
{
	public interface IWinAuthSettingsService : IDependency
	{
		WinAuthSettingsPart Retrieve();
	}
}