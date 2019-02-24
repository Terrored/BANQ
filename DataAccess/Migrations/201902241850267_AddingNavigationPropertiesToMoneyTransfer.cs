namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingNavigationPropertiesToMoneyTransfer : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.MoneyTransfers", name: "From_Id", newName: "FromId");
            RenameColumn(table: "dbo.MoneyTransfers", name: "To_Id", newName: "ToId");
            RenameIndex(table: "dbo.MoneyTransfers", name: "IX_From_Id", newName: "IX_FromId");
            RenameIndex(table: "dbo.MoneyTransfers", name: "IX_To_Id", newName: "IX_ToId");
            AddColumn("dbo.MoneyTransfers", "CreatedOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.MoneyTransfers", "DateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MoneyTransfers", "DateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.MoneyTransfers", "CreatedOn");
            RenameIndex(table: "dbo.MoneyTransfers", name: "IX_ToId", newName: "IX_To_Id");
            RenameIndex(table: "dbo.MoneyTransfers", name: "IX_FromId", newName: "IX_From_Id");
            RenameColumn(table: "dbo.MoneyTransfers", name: "ToId", newName: "To_Id");
            RenameColumn(table: "dbo.MoneyTransfers", name: "FromId", newName: "From_Id");
        }
    }
}
