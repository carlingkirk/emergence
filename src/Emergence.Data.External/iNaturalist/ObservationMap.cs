using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emergence.Data.External.iNaturalist
{
    public class ObservationMap
    {
        //public ObservationMap (Observation o)
        //{
        //    Id = o.id;
        //    CreatedOnDate = o.created_at;
        //    ObservedOn = o.observed_on_details.date;
        //    Location = new Geolocation(o.geojson?.coordinates, o.place_guess);
        //    Taxon = GetFullTaxon(o);
        //    ImageUrl = o.observation_photos.FirstOrDefault()?.photo?.url;
        //    Url = o.uri;
        //}

        //public int Id { get; set; }
        //public string Species => Taxon?.Species;
        //public string Genus => Taxon?.Genus;
        //public Taxon Taxon { get; set; }
        //public string ImageUrl { get; set; }
        //public string Url { get; set; }
        //public Geolocation Location { get; set; }
        //public string ObservedOn { get; set; }
        //public DateTime? ObservedOnDate => !string.IsNullOrEmpty(ObservedOn) ? (DateTime?)DateTime.Parse(ObservedOn) : null;
        //public DateTime? CreatedOnDate { get; set; }

        //public static Taxon GetFullTaxon(Observation observation)
        //{
        //    var taxon = new Taxon
        //    {
        //        Name = observation.taxon?.name,
        //        PreferredCommonName = observation.taxon?.preferred_common_name
        //    };

        //    var bestId = observation.identifications?.Where(id => id.own_observation).FirstOrDefault() ?? observation.identifications?.FirstOrDefault();
        //    var ancestors = bestId?.taxon?.ancestors?.ToList();

        //    taxon.Kingdom = ancestors?.Where(a => a.rank == Data.Rank.Kingdom).FirstOrDefault()?.name;
        //    taxon.Phylum = ancestors?.Where(a => a.rank == Data.Rank.Phylum).FirstOrDefault()?.name;
        //    taxon.Subphylum = ancestors?.Where(a => a.rank == Data.Rank.Subphylum).FirstOrDefault()?.name;
        //    taxon.Class = ancestors?.Where(a => a.rank == Data.Rank.Class).FirstOrDefault()?.name;
        //    taxon.Subclass = ancestors?.Where(a => a.rank == Data.Rank.Subclass).FirstOrDefault()?.name;
        //    taxon.Order = ancestors?.Where(a => a.rank == Data.Rank.Order).FirstOrDefault()?.name;
        //    taxon.Superfamily = ancestors?.Where(a => a.rank == Data.Rank.Superfamily).FirstOrDefault()?.name;
        //    taxon.Family = ancestors?.Where(a => a.rank == Data.Rank.Family).FirstOrDefault()?.name;
        //    taxon.Subfamily = ancestors?.Where(a => a.rank == Data.Rank.Subfamily).FirstOrDefault()?.name;
        //    taxon.Genus = ancestors?.Where(a => a.rank == Data.Rank.Genus).FirstOrDefault()?.name;
        //    taxon.Species = ancestors?.Where(a => a.rank == Data.Rank.Species).FirstOrDefault()?.name;

        //    return taxon;
        //}
    }
}
