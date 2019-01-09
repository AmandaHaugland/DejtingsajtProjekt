namespace DejtingsajtProjekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FriendIdNowInt : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Friends");
            AlterColumn("dbo.Friends", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Friends", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Friends");
            AlterColumn("dbo.Friends", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Friends", "Id");
        }
    }
}
