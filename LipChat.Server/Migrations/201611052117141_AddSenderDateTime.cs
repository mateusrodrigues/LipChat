namespace LipChat.Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSenderDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "DateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Messages", "Sender", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "Sender");
            DropColumn("dbo.Messages", "DateTime");
        }
    }
}
