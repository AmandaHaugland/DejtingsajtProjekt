namespace DejtingsajtProjekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamingColumnInFriend : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Friends", "SenderId", c => c.String());
            DropColumn("dbo.Friends", "FriendId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Friends", "FriendId", c => c.String());
            DropColumn("dbo.Friends", "SenderId");
        }
    }
}
