namespace Butterfly.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedrefreshtoken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tokens", "RefreshTokenValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tokens", "RefreshTokenValue");
        }
    }
}
