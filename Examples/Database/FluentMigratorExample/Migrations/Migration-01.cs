using FluentMigrator;

namespace NDepth.Examples.Database.FluentMigratorExample.Migrations
{
    [Migration(1)]
    public class Migration01 : Migration
    {
        public override void Up()
        {
            Create.Table("Customer")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("Name").AsString();
        }

        public override void Down()
        {
            Delete.Table("Customer");
        }
    }
}
