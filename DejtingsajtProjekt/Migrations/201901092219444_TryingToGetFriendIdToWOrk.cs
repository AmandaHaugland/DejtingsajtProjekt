namespace DejtingsajtProjekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TryingToGetFriendIdToWOrk : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Friends", "Sender", c => c.String());
            AddColumn("dbo.Friends", "Reciver", c => c.String());
            DropColumn("dbo.Friends", "SenderId");
            DropColumn("dbo.Friends", "ReciverId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Friends", "ReciverId", c => c.String());
            AddColumn("dbo.Friends", "SenderId", c => c.String());
            DropColumn("dbo.Friends", "Reciver");
            DropColumn("dbo.Friends", "Sender");
        }
    }
}
