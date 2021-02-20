namespace TheREALCarHouse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration_name02162021 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostID = c.Int(nullable: false, identity: true),
                        PostPrice = c.Double(nullable: false),
                        UserID = c.Int(nullable: false),
                        VehicleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: false)
                .ForeignKey("dbo.Vehicles", t => t.VehicleID, cascadeDelete: false)
                .Index(t => t.UserID)
                .Index(t => t.VehicleID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserFname = c.String(),
                        UserLname = c.String(),
                        UserEmail = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        VehicleID = c.Int(nullable: false, identity: true),
                        VehicleMake = c.String(),
                        VehicleModel = c.String(),
                        VehicleYear = c.String(),
                        VehicleColour = c.String(),
                        VehicleKMs = c.String(),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VehicleID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: false)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "VehicleID", "dbo.Vehicles");
            DropForeignKey("dbo.Posts", "UserID", "dbo.Users");
            DropForeignKey("dbo.Vehicles", "UserID", "dbo.Users");
            DropIndex("dbo.Vehicles", new[] { "UserID" });
            DropIndex("dbo.Posts", new[] { "VehicleID" });
            DropIndex("dbo.Posts", new[] { "UserID" });
            DropTable("dbo.Vehicles");
            DropTable("dbo.Users");
            DropTable("dbo.Posts");
        }
    }
}
