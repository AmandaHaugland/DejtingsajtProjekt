namespace DejtingsajtProjekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProfileModels", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProfileModels", "Description");
        }
    }
}
