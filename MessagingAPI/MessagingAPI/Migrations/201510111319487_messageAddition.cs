namespace MessagingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class messageAddition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "When", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "When");
        }
    }
}
