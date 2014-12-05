using System.Collections.Generic;

namespace AlexZh.WindowsAuthentication.ViewModels
{
	public class WinAuthSettingsPartViewModel
	{
		public bool EnableWindowsAuthentication { get; set; }
		public string EmailDomain { get; set; }
		public IList<RoleEntry> Roles { get; set; }
	}

	public class RoleEntry
	{
		public int RoleId { get; set; }
		public string Name { get; set; }
		public bool Selected { get; set; }
	}
}