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
                        Id = c.String(nullable: false, maxLength: 128),
                        FriendId = c.String(),
                        FriendshipAccepted = c.Boolean(nullable: false),
                        ReciverId = c.String(),
                        ProfileModel_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
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
