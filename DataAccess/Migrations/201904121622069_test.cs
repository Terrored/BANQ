namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Credits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BankAccountId = c.Int(nullable: false),
                        CreditAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PercentageRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalInstallments = c.Int(nullable: false),
                        InstallmentsAlreadyPaid = c.Int(nullable: false),
                        InstallmentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateTaken = c.DateTime(nullable: false),
                        NextInstallmentDate = c.DateTime(),
                        PaidInFull = c.Boolean(nullable: false),
                        Confirmed = c.Boolean(nullable: false),
                        ConfirmationDate = c.DateTime(),
                        BankAccount_ApplicationIdentityUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankAccounts", t => t.BankAccount_ApplicationIdentityUserId)
                .Index(t => t.BankAccount_ApplicationIdentityUserId);
            
            CreateTable(
                "dbo.Loans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BankAccountId = c.Int(nullable: false),
                        LoanAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LoanAmountLeft = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PercentageRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InstallmentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
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
            DropForeignKey("dbo.CreditInstallments", "CreditId", "dbo.Credits");
            DropForeignKey("dbo.Loans", "BankAccount_ApplicationIdentityUserId", "dbo.BankAccounts");
            DropForeignKey("dbo.Credits", "BankAccount_ApplicationIdentityUserId", "dbo.BankAccounts");
            DropIndex("dbo.LoanInstallments", new[] { "LoanId" });
            DropIndex("dbo.CreditInstallments", new[] { "CreditId" });
            DropIndex("dbo.Loans", new[] { "BankAccount_ApplicationIdentityUserId" });
            DropIndex("dbo.Credits", new[] { "BankAccount_ApplicationIdentityUserId" });
            DropTable("dbo.LoanInstallments");
            DropTable("dbo.CreditInstallments");
            DropTable("dbo.Loans");
            DropTable("dbo.Credits");
        }
    }
}
