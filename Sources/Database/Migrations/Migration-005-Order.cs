using FluentMigrator;

namespace NDepth.Database.Migrations
{
    [Migration(5)]
    public class Migration005Order : Migration
    {
        public override void Up()
        {
            Create.Table("Order")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey()
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("AccountFK").AsInt64().NotNullable().ForeignKey("Account", "Id")
                .WithColumn("Symbol").AsString().NotNullable()
                .WithColumn("Price").AsDecimal().NotNullable()
                .WithColumn("Volume").AsDecimal().NotNullable();

            Create.Index("IX_AccountFK").OnTable("Order")
                .OnColumn("AccountFK").Ascending();

            // Insert new key with initial value to HiLoTable table.
            Insert.IntoTable("HiLoTable").Row(new { TableName = "Order", NextHi = 1 });

            // Fill table with data.
            if (!this.IsSchemaOnly())
            {
                Insert.IntoTable("Order").Row(new { Id = 1, Version = 1, AccountFK = 1, Symbol = "EURUSD", Price = 1.123m, Volume = 1.0m });
                Insert.IntoTable("Order").Row(new { Id = 2, Version = 1, AccountFK = 1, Symbol = "EURUSD", Price = 1.456m, Volume = 10.0m });
                Insert.IntoTable("Order").Row(new { Id = 3, Version = 1, AccountFK = 1, Symbol = "EURUSD", Price = 1.789m, Volume = 100.0m });
                Insert.IntoTable("Order").Row(new { Id = 4, Version = 1, AccountFK = 2, Symbol = "EURAUD", Price = 1.412m, Volume = 1000.0m });
                Insert.IntoTable("Order").Row(new { Id = 5, Version = 1, AccountFK = 2, Symbol = "EURAUD", Price = 1.434m, Volume = 10000.0m });
                Insert.IntoTable("Order").Row(new { Id = 6, Version = 1, AccountFK = 3, Symbol = "GBPCHF", Price = 1.567m, Volume = 100000.0m });
            }
        }

        public override void Down()
        {
            // Remove corresponding key from HiLoTable table.
            Delete.FromTable("HiLoTable").Row(new { TableName = "Order" });

            Delete.Table("Order");
        }
    }
}
