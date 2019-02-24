namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNavigationPropertiesToBankAccount : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BankAccounts", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.BankAccounts", new[] { "Id" });
            DropPrimaryKey("dbo.BankAccounts");
            AddColumn("dbo.BankAccounts", "ApplicationIdentityUserId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.BankAccounts", "ApplicationIdentityUserId");
            CreateIndex("dbo.BankAccounts", "ApplicationIdentityUserId");
            AddForeignKey("dbo.BankAccounts", "ApplicationIdentityUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BankAccounts", "ApplicationIdentityUserId", "dbo.AspNetUsers");
            DropIndex("dbo.BankAccounts", new[] { "ApplicationIdentityUserId" });
            DropPrimaryKey("dbo.BankAccounts");
            DropColumn("dbo.BankAccounts", "ApplicationIdentityUserId");
            AddPrimaryKey("dbo.BankAccounts", "Id");
            CreateIndex("dbo.BankAccounts", "Id");
            AddForeignKey("dbo.BankAccounts", "Id", "dbo.AspNetUsers", "Id");
        }
    }
}
