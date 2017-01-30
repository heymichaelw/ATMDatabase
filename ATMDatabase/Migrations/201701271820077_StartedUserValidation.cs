namespace ATMDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StartedUserValidation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Deposits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        Time = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Withdrawals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        Time = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Withdrawals", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Deposits", "User_Id", "dbo.Users");
            DropIndex("dbo.Withdrawals", new[] { "User_Id" });
            DropIndex("dbo.Deposits", new[] { "User_Id" });
            DropTable("dbo.Withdrawals");
            DropTable("dbo.Deposits");
        }
    }
}
