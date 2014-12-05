using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace AlexZh.WindowsAuthentication.Models
{
	public class WinAuthSettingsPartRecord : ContentPartRecord
	{
		public virtual bool EnableWindowsAuthentication { get; set; }
		public virtual string EmailDomain { get; set; }
		public virtual string DefaultRoles { get; set; }
	}

	public class WinAuthSettingsPart : ContentPart<WinAuthSettingsPartRecord>
	{
		private IEnumerable<string> defaultRolesStorage;

		public bool EnableWindowsAuthentication
		{
			get { return Record.EnableWindowsAuthentication; }
			set { Record.EnableWindowsAuthentication = value; }
		}

		public string EmailDomain
		{
			get { return Record.EmailDomain; }
			set { Record.EmailDomain = value; }
		}

		public IEnumerable<string> DefaultRoles
		{
			get
			{
				return defaultRolesStorage ??
				       (defaultRolesStorage =
				        string.IsNullOrEmpty(Record.DefaultRoles) ? new string[] {} : Record.DefaultRoles.Split(','));
			}
			set
			{
				defaultRolesStorage = value;
				Record.DefaultRoles = defaultRolesStorage == null ? null : string.Join(",", defaultRolesStorage);
			}
		}
	}
}