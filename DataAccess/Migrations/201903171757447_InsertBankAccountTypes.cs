namespace DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InsertBankAccountTypes : DbMigration
    {
        public override void Up()
        {
            Sql("SET IDENTITY_INSERT dbo.BankAccountTypes ON " +
                "INSERT INTO dbo.BankAccountTypes (Id,Name) VALUES(1,'Regular')" +
                "INSERT INTO dbo.BankAccountTypes (Id,Name) VALUES(2,'Corporate')" +
                "INSERT INTO dbo.BankAccountTypes (Id,Name) VALUES(3,'Student')" +
                "INSERT INTO dbo.BankAccountTypes (Id,Name) VALUES(4,'Savings')");
        }

        public override void Down()
        {
        }
    }
}
