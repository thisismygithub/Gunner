namespace Gunner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fixtures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Opponent = c.String(),
                        Date = c.DateTime(nullable: false),
                        Result = c.String(),
                        Ground = c.String(),
                        Stadium = c.String(),
                        Attendance = c.Int(nullable: false),
                        Referee = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Fixtures");
        }
    }
}
