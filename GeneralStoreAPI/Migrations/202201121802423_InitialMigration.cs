namespace GeneralStoreAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Sku = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Cost = c.Double(nullable: false),
                        NumberInInventory = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Sku);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Products");
        }
    }
}
