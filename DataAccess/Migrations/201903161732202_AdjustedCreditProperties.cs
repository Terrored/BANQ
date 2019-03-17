namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustedCreditProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Credits", "PercentageRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Credits", "TotalInstallments", c => c.Int(nullable: false));
            AddColumn("dbo.Credits", "InstallmentsAlreadyPaid", c => c.Int(nullable: false));
            AddColumn("dbo.Credits", "InstallmentAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Credits", "DateTaken", c => c.DateTime(nullable: false));
            AddColumn("dbo.Credits", "NextInstallmentDate", c => c.DateTime());
            AddColumn("dbo.Credits", "PaidInFull", c => c.Boolean(nullable: false));
            DropColumn("dbo.Credits", "Repayment");
            DropColumn("dbo.Credits", "InstallmentCount");
            DropColumn("dbo.Credits", "InstallmentsMade");
            DropColumn("dbo.Credits", "MoneyRepayed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Credits", "MoneyRepayed", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Credits", "InstallmentsMade", c => c.Int(nullable: false));
            AddColumn("dbo.Credits", "InstallmentCount", c => c.Int(nullable: false));
            AddColumn("dbo.Credits", "Repayment", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Credits", "PaidInFull");
            DropColumn("dbo.Credits", "NextInstallmentDate");
            DropColumn("dbo.Credits", "DateTaken");
            DropColumn("dbo.Credits", "InstallmentAmount");
            DropColumn("dbo.Credits", "InstallmentsAlreadyPaid");
            DropColumn("dbo.Credits", "TotalInstallments");
            DropColumn("dbo.Credits", "PercentageRate");
        }
    }
}
