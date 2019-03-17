namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Loan : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Loans",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        BankAccountId = c.Int(nullable: false),
                        LoanAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PercentageRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalInstallments = c.Int(nullable: false),
                        InstallmentsLeft = c.Int(nullable: false),
                        DateTaken = c.DateTime(nullable: false),
                        NextInstallmentDate = c.DateTime(),
                        Repayment = c.Boolean(nullable: false),
                        RepaymentDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankAccounts", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.BankAccounts", "LoanId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Loans", "Id", "dbo.BankAccounts");
            DropIndex("dbo.Loans", new[] { "Id" });
            DropColumn("dbo.BankAccounts", "LoanId");
            DropTable("dbo.Loans");
        }
    }
}
