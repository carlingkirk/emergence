using Microsoft.EntityFrameworkCore.Migrations;

namespace Emergence.Data.Migrations
{
    public static class MigrationSeeds
    {
        public static void AddEmergenceSeedData(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into Lifeforms (Id, CommonName, ScientificName) " +
                "values (1,'Butterfly Milkweed','Ascepias tuberosa')");

            migrationBuilder.Sql("insert into Inventories (Id, UserId) " +
                "values (1,'6566e794-c7ce-4939-9158-a7d42a6a47ea')");

            migrationBuilder.Sql("insert into InventoryItems (Id,InventoryId,ItemType,Name,Quantity,Status,DateAcquired,DateCreated,DateModified) " +
                "values (1,1,'Specimen','Butterfly Milkweed',1,'InUse','2020-07-01 19:15:45.000','2020-07-01 19:15:45.000','2020-07-01 19:15:45.000')");

            migrationBuilder.Sql("insert into Specimens (Id,LifeformId,InventoryItemId,SpecimenStage,DateCreated,DateModified) " +
                "values (1,1,1,'InGround','2020-07-01 19:15:45.000','2020-07-01 19:15:45.000')");

            migrationBuilder.Sql("insert into Origins (Id,Name,Type,Uri,UserId,DateCreated,DateModified) " +
                "values (1,'Plant Finder - Missouri Botanical Garden','Website','http://www.missouribotanicalgarden.org/plantfinder/plantfindersearch.aspx'," +
                "'6566e794-c7ce-4939-9158-a7d42a6a47ea','2020-07-01 19:15:45.000','2020-07-01 19:15:45.000')");
        }
    }
}
