namespace SearchApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attributes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 4000),
                        FreebaseName = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 4000),
                        FreebaseName = c.String(nullable: false, maxLength: 4000),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Domains",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 4000),
                        FreebaseName = c.String(nullable: false, maxLength: 4000),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TypeToAttribute",
                c => new
                    {
                        TypeID = c.Int(nullable: false),
                        AttributeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TypeID, t.AttributeID })
                .ForeignKey("dbo.Types", t => t.TypeID, cascadeDelete: true)
                .ForeignKey("dbo.Attributes", t => t.AttributeID, cascadeDelete: true)
                .Index(t => t.TypeID)
                .Index(t => t.AttributeID);
            
            CreateTable(
                "dbo.DomainToType",
                c => new
                    {
                        DomainID = c.Int(nullable: false),
                        TypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DomainID, t.TypeID })
                .ForeignKey("dbo.Domains", t => t.DomainID, cascadeDelete: true)
                .ForeignKey("dbo.Types", t => t.TypeID, cascadeDelete: true)
                .Index(t => t.DomainID)
                .Index(t => t.TypeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DomainToType", "TypeID", "dbo.Types");
            DropForeignKey("dbo.DomainToType", "DomainID", "dbo.Domains");
            DropForeignKey("dbo.TypeToAttribute", "AttributeID", "dbo.Attributes");
            DropForeignKey("dbo.TypeToAttribute", "TypeID", "dbo.Types");
            DropIndex("dbo.DomainToType", new[] { "TypeID" });
            DropIndex("dbo.DomainToType", new[] { "DomainID" });
            DropIndex("dbo.TypeToAttribute", new[] { "AttributeID" });
            DropIndex("dbo.TypeToAttribute", new[] { "TypeID" });
            DropTable("dbo.DomainToType");
            DropTable("dbo.TypeToAttribute");
            DropTable("dbo.Domains");
            DropTable("dbo.Types");
            DropTable("dbo.Attributes");
        }
    }
}
