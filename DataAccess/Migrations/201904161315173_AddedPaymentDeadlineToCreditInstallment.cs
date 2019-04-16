namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPaymentDeadlineToCreditInstallment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CreditInstallments", "PaymentDeadline", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CreditInstallments", "PaidOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CreditInstallments", "PaidOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.CreditInstallments", "PaymentDeadline");
        }
    }
}
