using FluentMigrator;

namespace NDepth.Database.Migrations
{
    [Migration(2)]
    public class Migration002MonitoringEvent : Migration
    {
        public override void Up()
        {
            Create.Table("MonitoringEvent")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey()
                .WithColumn("Timestamp").AsCustom("datetime2").NotNullable()
                .WithColumn("Machine").AsString().NotNullable()
                .WithColumn("Module").AsString().NotNullable()
                .WithColumn("Component").AsString().NotNullable()
                .WithColumn("Severity").AsInt16().NotNullable()
                .WithColumn("Title").AsString().NotNullable()
                .WithColumn("Description").AsString().NotNullable();

            Create.Index("IX_Timestamp_Id_Machine_Module_Component").OnTable("MonitoringEvent")
                .OnColumn("Timestamp").Ascending()
                .OnColumn("Id").Ascending()
                .OnColumn("Machine").Ascending()
                .OnColumn("Module").Ascending()
                .OnColumn("Component").Ascending();

            // Insert new key with initial value to HiLoTable table.
            Insert.IntoTable("HiLoTable").Row(new { TableName = "MonitoringEvent", NextHi = 1 });
        }

        public override void Down()
        {
            // Remove corresponding key from HiLoTable table.
            Delete.FromTable("HiLoTable").Row(new { TableName = "MonitoringEvent" });

            Delete.Table("MonitoringEvent");
        }
    }
}
