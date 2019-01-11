namespace DejtingsajtProjekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        FriendId = c.Int(nullable: false, identity: true),
                        Sender = c.String(),
                        FriendshipAccepted = c.Boolean(nullable: false),
                        Reciver = c.String(),
                        ProfileModel_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FriendId)
                .ForeignKey("dbo.ProfileModels", t => t.ProfileModel_UserId)
                .Index(t => t.ProfileModel_UserId);
            
            CreateTable(
                "dbo.ProfileModels",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Firstname = c.String(),
                        Lastname = c.String(),
                        Birthday = c.DateTime(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        Sender = c.String(),
                        MessageText = c.String(),
                        Reciver = c.String(),
                        ProfileModel_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.ProfileModels", t => t.ProfileModel_UserId)
                .Index(t => t.ProfileModel_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "ProfileModel_UserId", "dbo.ProfileModels");
            DropForeignKey("dbo.Friends", "ProfileModel_UserId", "dbo.ProfileModels");
            DropIndex("dbo.Messages", new[] { "ProfileModel_UserId" });
            DropIndex("dbo.Friends", new[] { "ProfileModel_UserId" });
            DropTable("dbo.Messages");
            DropTable("dbo.ProfileModels");
            DropTable("dbo.Friends");
        }
    }
}
