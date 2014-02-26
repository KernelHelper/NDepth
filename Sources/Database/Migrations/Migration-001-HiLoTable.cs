using FluentMigrator;

namespace NDepth.Database.Migrations
{
    [Migration(1)]
    public class Migration001HiLoTable : Migration
    {
        public override void Up()
        {
            Create.Table("HiLoTable")
                .WithColumn("TableName").AsString(50).NotNullable().PrimaryKey()
                .WithColumn("NextHi").AsInt64().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("HiLoTable");
        }    
    }
}
