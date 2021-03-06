namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSexAsUserProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Sex", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Sex");
        }
    }
}
