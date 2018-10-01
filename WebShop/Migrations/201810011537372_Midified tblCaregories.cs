namespace WebShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MidifiedtblCaregories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblCategories", "ParentId", c => c.Int());
            CreateIndex("dbo.tblCategories", "ParentId");
            AddForeignKey("dbo.tblCategories", "ParentId", "dbo.tblCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblCategories", "ParentId", "dbo.tblCategories");
            DropIndex("dbo.tblCategories", new[] { "ParentId" });
            DropColumn("dbo.tblCategories", "ParentId");
        }
    }
}
