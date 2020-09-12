using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class SynonymExtensions
    {
        public static Models.Synonym AsModel(this Synonym source) => new Models.Synonym
        {
            SynonymId = source.Id,
            Name = source.Name,
            Rank = source.Rank,
            Language = source.Language,
            DateUpdated = source.DateUpdated,
            TaxonId = source.TaxonId,
            Taxon = source.Taxon != null ? source.Taxon.AsModel() : new Models.Taxon { TaxonId = source.TaxonId },
            Origin = source.Origin != null ? source.Origin.AsModel() : source.OriginId.HasValue ? new Models.Origin { OriginId = source.OriginId.Value } : null
        };

        public static Synonym AsStore(this Models.Synonym source) => new Synonym
        {
            Id = source.SynonymId,
            Name = source.Name,
            Rank = source.Rank,
            Language = source.Language,
            DateUpdated = source.DateUpdated,
            TaxonId = source.Taxon?.TaxonId ?? source.TaxonId,
            OriginId = source.Origin?.OriginId
        };
    }
}
