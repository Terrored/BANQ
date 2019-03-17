namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoanInstallments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LoanInstallments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LoanId = c.Int(nullable: false),
                        InstallmentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaidOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Loans", t => t.LoanId)
                .Index(t => t.LoanId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LoanInstallments", "LoanId", "dbo.Loans");
            DropIndex("dbo.LoanInstallments", new[] { "LoanId" });
            DropTable("dbo.LoanInstallments");
        }
    }
}
