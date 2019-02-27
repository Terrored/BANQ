namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetSexToRequiredProperty : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Sex", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Sex", c => c.String());
        }
    }
}
