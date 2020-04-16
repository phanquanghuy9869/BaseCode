namespace Kpi.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AspNetUser_Col : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsDeleted");
            DropColumn("dbo.AspNetUsers", "Name");
        }
    }
}
