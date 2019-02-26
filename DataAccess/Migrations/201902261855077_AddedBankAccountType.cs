namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBankAccountType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankAccountTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.BankAccounts", "BankAccountTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.BankAccounts", "BankAccountTypeId");
            AddForeignKey("dbo.BankAccounts", "BankAccountTypeId", "dbo.BankAccountTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BankAccounts", "BankAccountTypeId", "dbo.BankAccountTypes");
            DropIndex("dbo.BankAccounts", new[] { "BankAccountTypeId" });
            DropColumn("dbo.BankAccounts", "BankAccountTypeId");
            DropTable("dbo.BankAccountTypes");
        }
    }
}
