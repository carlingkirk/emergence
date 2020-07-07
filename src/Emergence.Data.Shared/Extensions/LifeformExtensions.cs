using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class LifeformExtensions
    {
        public static Models.Lifeform AsModel(this Lifeform source) => new Models.Lifeform
        {
            LifeformId = source.Id,
            CommonName = source.CommonName,
            ScientificName = source.ScientificName
        };

        public static Lifeform AsStore(this Models.Lifeform source) => new Lifeform
        {
            Id = source.LifeformId,
            CommonName = source.CommonName,
            ScientificName = source.ScientificName
        };
    }
}