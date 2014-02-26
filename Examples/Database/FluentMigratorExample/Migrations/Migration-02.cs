using System;
using FluentMigrator;

namespace NDepth.Examples.Database.FluentMigratorExample.Migrations
{
    [Migration(2)]
    public class Migration02 : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Customer").Row(
                new
                {
                    Id = Guid.NewGuid(),
                    Name = "Andy"
                }).Row(
                new
                {
                    Id = Guid.NewGuid(),
                    Name = "Liza"
                });
        }

        public override void Down()
        {
            Delete.FromTable("Customer").AllRows();
        }
    }
}
