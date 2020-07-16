using System;
using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Stores;
using Newtonsoft.Json;

namespace Emergence.Data.Shared.Extensions
{
    public static class PlantInfoExtensions
    {
        public static Models.PlantInfo AsModel(this PlantInfo source)
        {
            List<Models.StratificationStage> stratificationStages = null;
            if (!string.IsNullOrEmpty(source.StratificationStages))
            {
                stratificationStages = JsonConvert.DeserializeObject<List<Models.StratificationStage>>(source.StratificationStages);
            }

            var minimumZoneNumber = 0;
            var maximumZoneNumber = 0;
            var minimumZoneLetter = "";
            var maximumZoneLetter = "";
            if (!string.IsNullOrEmpty(source.MinimumZone))
            {
                int.TryParse(source.MinimumZone.First().ToString(), out minimumZoneNumber);

                if (source.MinimumZone.Length == 2)
                {
                    minimumZoneLetter = source.MinimumZone.Last().ToString();
                }
            }

            if (!string.IsNullOrEmpty(source.MaximumZone))
            {
                int.TryParse(source.MaximumZone.First().ToString(), out maximumZoneNumber);
                if (source.MaximumZone.Length == 2)
                {
                    maximumZoneLetter = source.MaximumZone.Last().ToString();
                }
            }

            return new Models.PlantInfo
            {
                PlantInfoId = source.Id,
                CommonName = source.CommonName,
                ScientificName = source.ScientificName,
                LifeformId = source.LifeformId,
                Origin = source.OriginId.HasValue ? new Models.Origin { OriginId = source.OriginId.Value } : null,
                BloomTime = new Models.BloomTime
                {
                    MinimumBloomTime = (Models.Month)source.MinimumBloomTime,
                    MaximumBloomTime = (Models.Month)source.MaximumBloomTime
                },
                Height = new Models.Height
                {
                    MinimumHeight = source.MinimumHeight,
                    MaximumHeight = source.MaximumHeight,
                    Unit = Enum.Parse<Models.DistanceUnit>(source.HeightUnit)
                },
                Spread = new Models.Spread
                {
                    MinimumSpread = source.MinimumSpread,
                    MaximumSpread = source.MaximumSpread,
                    Unit = Enum.Parse<Models.DistanceUnit>(source.SpreadUnit)
                },
                Requirements = new Models.Requirements
                {
                    LightRequirements = new Models.LightRequirements
                    {
                        MinimumLight = Enum.Parse<Models.LightType>(source.MinimumLight),
                        MaximumLight = Enum.Parse<Models.LightType>(source.MaximumLight)
                    },
                    WaterRequirements = new Models.WaterRequirements
                    {
                        MinimumWater = Enum.Parse<Models.WaterType>(source.MinimumWater),
                        MaximumWater = Enum.Parse<Models.WaterType>(source.MaximumWater),
                    },
                    SoilRequirements = null,
                    StratificationStages = stratificationStages,
                    ZoneRequirements = new Models.ZoneRequirements
                    {
                        MinimumZone = new Models.Zone
                        {
                            Number = minimumZoneNumber,
                            Letter = minimumZoneLetter
                        },
                        MaximumZone = new Models.Zone
                        {
                            Number = maximumZoneNumber,
                            Letter = maximumZoneLetter
                        }
                    }
                },
                Taxon = source.TaxonId.HasValue ? new Models.Taxon { TaxonId = source.TaxonId.Value } : null,
                CreatedBy = source.CreatedBy,
                DateCreated = source.DateCreated,
                DateModified = source.DateModified
            };
        }

        public static PlantInfo AsStore(this Models.PlantInfo source) => new PlantInfo
        {
            Id = source.PlantInfoId,
            CommonName = source.CommonName,
            ScientificName = source.ScientificName,
            OriginId = source.Origin?.OriginId,
            LifeformId = source.LifeformId,
            MinimumBloomTime = (short)source.BloomTime.MinimumBloomTime,
            MaximumBloomTime = (short)source.BloomTime.MaximumBloomTime,
            MinimumHeight = source.Height.MinimumHeight,
            MaximumHeight = source.Height.MaximumHeight,
            HeightUnit = source.Height.Unit.ToString(),
            MinimumSpread = source.Spread.MinimumSpread,
            MaximumSpread = source.Spread.MaximumSpread,
            SpreadUnit = source.Spread.Unit.ToString(),
            MinimumLight = source.Requirements.LightRequirements.MinimumLight.ToString(),
            MaximumLight = source.Requirements.LightRequirements.MaximumLight.ToString(),
            MinimumWater = source.Requirements.WaterRequirements.MinimumWater.ToString(),
            MaximumWater = source.Requirements.WaterRequirements.MaximumWater.ToString(),
            MinimumZone = source.Requirements.ZoneRequirements.MinimumZone.Letter + source.Requirements.ZoneRequirements.MinimumZone.Number,
            MaximumZone = source.Requirements.ZoneRequirements.MaximumZone.Letter + source.Requirements.ZoneRequirements.MaximumZone.Number,
            StratificationStages = source.Requirements?.StratificationStages != null ? JsonConvert.SerializeObject(source.Requirements.StratificationStages) : null,
            Preferred = source.Preferred,
            TaxonId = source.Taxon?.TaxonId,
            CreatedBy = source.CreatedBy,
            DateCreated = source.DateCreated ?? DateTime.UtcNow,
            DateModified = source.DateModified
        };
    }
}
