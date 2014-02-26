using FluentMigrator;

namespace NDepth.Database.Migrations
{
    [Migration(4)]
    public class Migration004Account : Migration
    {
        public override void Up()
        {
            Create.Table("Account")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey()
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("Name").AsString().NotNullable();

            // Insert new key with initial value to HiLoTable table.
            Insert.IntoTable("HiLoTable").Row(new { TableName = "Account", NextHi = 1 });

            // Fill table with data.
            if (!this.IsSchemaOnly())
            {
                Insert.IntoTable("Account").Row(new { Id = 1, Version = 1, Name = "Account1" });
                Insert.IntoTable("Account").Row(new { Id = 2, Version = 1, Name = "Account2" });
                Insert.IntoTable("Account").Row(new { Id = 3, Version = 1, Name = "Account3" });
                Insert.IntoTable("Account").Row(new { Id = 4, Version = 1, Name = "Account4" });
            }
        }

        public override void Down()
        {
            // Remove corresponding key from HiLoTable table.
            Delete.FromTable("HiLoTable").Row(new { TableName = "Account" });

            Delete.Table("Account");
        }
    }
}
