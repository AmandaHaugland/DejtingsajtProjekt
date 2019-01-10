namespace DejtingsajtProjekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamingFriendKey : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Friends");
            AddColumn("dbo.Friends", "FriendId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Friends", "FriendId");
            DropColumn("dbo.Friends", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Friends", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Friends");
            DropColumn("dbo.Friends", "FriendId");
            AddPrimaryKey("dbo.Friends", "Id");
        }
    }
}
