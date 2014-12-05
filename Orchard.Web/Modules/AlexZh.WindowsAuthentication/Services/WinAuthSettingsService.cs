using System.Linq;
using AlexZh.WindowsAuthentication.Models;
using Orchard.Caching;
using Orchard.ContentManagement;

namespace AlexZh.WindowsAuthentication.Services
{
	public class WinUserSettingsService : IWinAuthSettingsService
	{
		private readonly ICacheManager cacheManager;
		private readonly IContentManager contentManager;
		private readonly ISignals signals;

		public WinUserSettingsService(IContentManager contentManager, ICacheManager cacheManager, ISignals signals)
		{
			this.contentManager = contentManager;
			this.cacheManager = cacheManager;
			this.signals = signals;
		}

		#region IWinAuthSettingsService Members

		public WinAuthSettingsPart Retrieve()
		{
			return cacheManager.Get("AlexZh.WindowsAuthentication.Settings",
			                        ctx =>
			                        	{
			                        		ctx.Monitor(signals.When("AlexZh.WindowsAuthentication.SettingsChanged"));
			                        		WinAuthSettingsPart settings =
			                        			contentManager.Query<WinAuthSettingsPart, WinAuthSettingsPartRecord>().List().
			                        				FirstOrDefault();
			                        		if (settings != null)
			                        		{
			                        			return settings;
			                        		}
			                        		return new WinAuthSettingsPart {Record = new WinAuthSettingsPartRecord()};
			                        	});
		}

		#endregion
	}
}