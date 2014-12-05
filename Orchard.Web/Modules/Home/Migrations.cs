using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Indexing;
namespace Home {
    public class Migrations : DataMigrationImpl {

        public int Create() {
			// Creating table HomePartRecord
			SchemaBuilder.CreateTable("HomePartRecord", table => table
				.ContentPartRecord()
				.Column("name", DbType.String)
				.Column("tooltip", DbType.String)
				.Column("linkref", DbType.String)
				.Column("pichref", DbType.String)
			);



            return 1;
        }
        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterPartDefinition("HomePart",
              builder => builder.Attachable());
            return 2;
        }
        public int UpdateFrom2()
        {
            ContentDefinitionManager.AlterTypeDefinition("Home", cfg => cfg
              .WithPart("CommonPart")
              .WithPart("RoutePart")
              .WithPart("BodyPart")
              .WithPart("ProductPart")
              .WithPart("CommentsPart")
              .WithPart("TagsPart")
              .WithPart("LocalizationPart")
              .Creatable()
              .Indexed());
            return 3;
        }
    }
}