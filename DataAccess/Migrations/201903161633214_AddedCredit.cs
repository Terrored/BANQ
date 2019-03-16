namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCredit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Credits",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        BankAccountId = c.Int(nullable: false),
                        CreditAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Repayment = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InstallmentCount = c.Int(nullable: false),
                        InstallmentsMade = c.Int(nullable: false),
                        MoneyRepayed = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankAccounts", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.BankAccounts", "CreditId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Credits", "Id", "dbo.BankAccounts");
            DropIndex("dbo.Credits", new[] { "Id" });
            DropColumn("dbo.BankAccounts", "CreditId");
            DropTable("dbo.Credits");
        }
    }
}
