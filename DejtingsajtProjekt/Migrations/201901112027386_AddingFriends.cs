namespace DejtingsajtProjekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFriends : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Friends", "ProfileModel_UserId", "dbo.ProfileModels");
            DropIndex("dbo.Friends", new[] { "ProfileModel_UserId" });
            DropTable("dbo.Friends");
        }
    }
}
