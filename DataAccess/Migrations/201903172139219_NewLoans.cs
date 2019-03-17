namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewLoans : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Loans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BankAccountId = c.Int(nullable: false),
                        LoanAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PercentageRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalInstallments = c.Int(nullable: false),
                        InstallmentsLeft = c.Int(nullable: false),
                        DateTaken = c.DateTime(nullable: false),
                        NextInstallmentDate = c.DateTime(),
                        Repayment = c.Boolean(nullable: false),
                        RepaymentDate = c.DateTime(),
                        BankAccount_ApplicationIdentityUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankAccounts", t => t.BankAccount_ApplicationIdentityUserId)
                .Index(t => t.BankAccount_ApplicationIdentityUserId);
            
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
            DropForeignKey("dbo.Loans", "BankAccount_ApplicationIdentityUserId", "dbo.BankAccounts");
            DropIndex("dbo.LoanInstallments", new[] { "LoanId" });
            DropIndex("dbo.Loans", new[] { "BankAccount_ApplicationIdentityUserId" });
            DropTable("dbo.LoanInstallments");
            DropTable("dbo.Loans");
        }
    }
}
