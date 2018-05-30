namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnonymousPost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Title", c => c.String());
            AddColumn("dbo.Posts", "IsAnonymous", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "IsAnonymous");
            DropColumn("dbo.Posts", "Title");
        }
    }
}
