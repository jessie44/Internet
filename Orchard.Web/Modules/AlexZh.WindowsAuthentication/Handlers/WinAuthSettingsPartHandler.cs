using AlexZh.WindowsAuthentication.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;

namespace AlexZh.WindowsAuthentication.Handlers
{
	[UsedImplicitly]
	public class WinUserSettingsPartHandler : ContentHandler
	{
		public WinUserSettingsPartHandler(IRepository<WinAuthSettingsPartRecord> repository)
		{
			T = NullLocalizer.Instance;
			Filters.Add(new ActivatingFilter<WinAuthSettingsPart>("Site"));
			Filters.Add(StorageFilter.For(repository));
		}

		public Localizer T { get; set; }

		protected override void GetItemMetadata(GetContentItemMetadataContext context)
		{
			if (context.ContentItem.ContentType != "Site")
				return;
			base.GetItemMetadata(context);
			context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Users")));
		}
	}
}