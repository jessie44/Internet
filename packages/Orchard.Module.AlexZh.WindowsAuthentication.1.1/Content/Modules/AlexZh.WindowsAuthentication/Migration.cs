using System;
using System.Data;
using AlexZh.WindowsAuthentication.Models;
using Orchard.ContentManagement.Records;
using Orchard.Data;
using Orchard.Data.Migration;

namespace AlexZh.WindowsAuthentication
{
	public class Migration : DataMigrationImpl
	{
		private readonly IRepository<WinAuthSettingsPartRecord> repository;

		public Migration(IRepository<WinAuthSettingsPartRecord> repository)
		{
			this.repository = repository;
		}

		public int Create()
		{
			SchemaBuilder.CreateTable("WinAuthSettingsPartRecord",
			                          table => table
			                                   	.ContentPartRecord()
			                                   	.Column<bool>("EnableWindowsAuthentication", c => c.WithDefault(true))
			                                   	.Column<string>("DefaultRoles", c => c.Unlimited())
			                                   	.Column("EmailDomain", DbType.String, c => c.Unlimited())
				);

			if (repository == null)
				throw new InvalidOperationException("Couldn't find settings repository.");

			repository.Create(new WinAuthSettingsPartRecord
			                  	{
			                  		EnableWindowsAuthentication = true,
			                  		DefaultRoles = string.Empty,
			                  		EmailDomain = string.Empty,
			                  		ContentItemRecord = new ContentItemRecord {Id = 1}
			                  	});

			return 1;
		}
	}
}