namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateGenresTable : DbMigration
    {
        public override void Up()
        {
            Sql("Insert INTO Genres (Name) VALUES ('Jazz')");
            Sql("Insert INTO Genres (Name) VALUES ('Blues')");
            Sql("Insert INTO Genres (Name) VALUES ('Rock')");
            Sql("Insert INTO Genres (Name) VALUES ('Country')");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Genres WHERE Name = 'Jazz'");
            Sql("DELETE FROM Genres WHERE Name = 'Blues'");
            Sql("DELETE FROM Genres WHERE Name = 'Rock'");
            Sql("DELETE FROM Genres WHERE Name = 'Country'");
        }
    }
}
