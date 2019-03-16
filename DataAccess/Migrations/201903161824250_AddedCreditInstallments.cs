namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCreditInstallments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreditInstallments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreditId = c.Int(nullable: false),
                        InstallmentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaidOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Credits", t => t.CreditId)
                .Index(t => t.CreditId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CreditInstallments", "CreditId", "dbo.Credits");
            DropIndex("dbo.CreditInstallments", new[] { "CreditId" });
            DropTable("dbo.CreditInstallments");
        }
    }
}
