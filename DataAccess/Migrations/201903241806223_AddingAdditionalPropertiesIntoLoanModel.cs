namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingAdditionalPropertiesIntoLoanModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Loans", "LoanAmountLeft", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Loans", "InstallmentAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Loans", "InstallmentAmount");
            DropColumn("dbo.Loans", "LoanAmountLeft");
        }
    }
}
