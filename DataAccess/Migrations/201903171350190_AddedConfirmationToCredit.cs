namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedConfirmationToCredit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Credits", "Confirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Credits", "ConfirmationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Credits", "ConfirmationDate");
            DropColumn("dbo.Credits", "Confirmed");
        }
    }
}
