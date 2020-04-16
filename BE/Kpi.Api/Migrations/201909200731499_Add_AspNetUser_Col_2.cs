namespace Kpi.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AspNetUser_Col_2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Name", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Name", c => c.String());
        }
    }
}
