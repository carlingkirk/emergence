namespace Emergence.Data.Shared.Stores
{
    public class PlantSynonym
    {
        public int Id { get; set; }
        public int LifeformId { get; set; }
        public string Synonym { get; set; }

        public Lifeform Lifeform { get; set; }
    }
}
