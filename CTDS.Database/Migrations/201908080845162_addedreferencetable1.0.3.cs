namespace CTDS.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedreferencetable103 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ReferenceTables", "Reference", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReferenceTables", "Reference", c => c.String());
        }
    }
}
