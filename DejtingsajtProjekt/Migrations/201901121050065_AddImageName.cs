namespace DejtingsajtProjekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProfileModels", "ImageName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProfileModels", "ImageName");
        }
    }
}
