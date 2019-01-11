namespace DejtingsajtProjekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingMessages : DbMigration
    {
        public override void Up()
        {
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
            DropIndex("dbo.Messages", new[] { "ProfileModel_UserId" });
            DropTable("dbo.Messages");
        }
    }
}
