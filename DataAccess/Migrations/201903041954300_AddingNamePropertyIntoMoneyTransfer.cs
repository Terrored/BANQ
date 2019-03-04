namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingNamePropertyIntoMoneyTransfer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MoneyTransfers", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MoneyTransfers", "Name");
        }
    }
}
