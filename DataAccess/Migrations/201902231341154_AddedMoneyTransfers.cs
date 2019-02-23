namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMoneyTransfers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MoneyTransfers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        CashAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        From_Id = c.Int(nullable: false),
                        To_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.From_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.To_Id)
                .Index(t => t.From_Id)
                .Index(t => t.To_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MoneyTransfers", "To_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.MoneyTransfers", "From_Id", "dbo.AspNetUsers");
            DropIndex("dbo.MoneyTransfers", new[] { "To_Id" });
            DropIndex("dbo.MoneyTransfers", new[] { "From_Id" });
            DropTable("dbo.MoneyTransfers");
        }
    }
}
