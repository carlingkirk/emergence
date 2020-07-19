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

            Models.Zone minimumZone = null;
            Models.Zone maximumZone = null;
            if (!string.IsNullOrEmpty(source.MinimumZone))
            {
                int.TryParse(source.MinimumZone.First().ToString(), out var minimumZoneNumber);

                if (source.MinimumZone.Length == 2)
                {
                    var minimumZoneLetter = source.MinimumZone.Last().ToString();
                    minimumZone = new Models.Zone
                    {
                        Number = minimumZoneNumber,
                        Letter = minimumZoneLetter
                    };
                }
            }

            if (!string.IsNullOrEmpty(source.MaximumZone))
            {
                int.TryParse(source.MaximumZone.First().ToString(), out var maximumZoneNumber);
                if (source.MaximumZone.Length == 2)
                {
                    var maximumZoneLetter = source.MaximumZone.Last().ToString();
                    maximumZone = new Models.Zone
                    {
                        Number = maximumZoneNumber,
                        Letter = maximumZoneLetter
                    };
                }
            }

            return new Models.PlantInfo
            {
                PlantInfoId = source.Id,
                CommonName = source.CommonName,
                ScientificName = source.ScientificName,
                LifeformId = source.LifeformId,
                Lifeform = new Models.Lifeform { LifeformId = source.LifeformId },
                Origin = source.OriginId.HasValue ? new Models.Origin { OriginId = source.OriginId.Value } : null,
                BloomTime = new Models.BloomTime
                {
                    MinimumBloomTime = source.MinimumBloomTime.HasValue ? (Models.Month)source.MinimumBloomTime.Value : Models.Month.Unknown,
                    MaximumBloomTime = source.MaximumBloomTime.HasValue ? (Models.Month)source.MaximumBloomTime.Value : Models.Month.Unknown
                },
                Height = new Models.Height
                {
                    MinimumHeight = source.MinimumHeight,
                    MaximumHeight = source.MaximumHeight,
                    Unit = !string.IsNullOrEmpty(source.HeightUnit) ? Enum.Parse<Models.DistanceUnit>(source.HeightUnit) : Models.DistanceUnit.Unknown
                },
                Spread = new Models.Spread
                {
                    MinimumSpread = source.MinimumSpread,
                    MaximumSpread = source.MaximumSpread,
                    Unit = !string.IsNullOrEmpty(source.SpreadUnit) ? Enum.Parse<Models.DistanceUnit>(source.SpreadUnit) : Models.DistanceUnit.Unknown
                },
                Requirements = new Models.Requirements
                {
                    LightRequirements = new Models.LightRequirements
                    {
                        MinimumLight = !string.IsNullOrEmpty(source.MinimumLight) ? Enum.Parse<Models.LightType>(source.MinimumLight) : Models.LightType.Unknown,
                        MaximumLight = !string.IsNullOrEmpty(source.MaximumLight) ? Enum.Parse<Models.LightType>(source.MaximumLight) : Models.LightType.Unknown
                    },
                    WaterRequirements = new Models.WaterRequirements
                    {
                        MinimumWater = !string.IsNullOrEmpty(source.MinimumWater) ? Enum.Parse<Models.WaterType>(source.MinimumWater) : Models.WaterType.Unknown,
                        MaximumWater = !string.IsNullOrEmpty(source.MaximumWater) ? Enum.Parse<Models.WaterType>(source.MaximumWater) : Models.WaterType.Unknown,
                    },
                    SoilRequirements = null,
                    StratificationStages = stratificationStages,
                    ZoneRequirements = new Models.ZoneRequirements
                    {
                        MinimumZone = minimumZone,
                        MaximumZone = maximumZone
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
            LifeformId = source.Lifeform?.LifeformId ?? source.LifeformId,
            MinimumBloomTime = (short?)source.BloomTime?.MinimumBloomTime,
            MaximumBloomTime = (short?)source.BloomTime?.MaximumBloomTime,
            MinimumHeight = source.Height?.MinimumHeight,
            MaximumHeight = source.Height?.MaximumHeight,
            HeightUnit = source.Height?.Unit != Models.DistanceUnit.Unknown ? source.Height?.Unit.ToString() : null,
            MinimumSpread = source.Spread?.MinimumSpread,
            MaximumSpread = source.Spread?.MaximumSpread,
            SpreadUnit = source.Spread?.Unit != Models.DistanceUnit.Unknown ? source.Spread?.Unit.ToString() : null,
            MinimumLight = source.Requirements?.LightRequirements?.MinimumLight != Models.LightType.Unknown ? source.Requirements?.LightRequirements?.MinimumLight.ToString() : null,
            MaximumLight = source.Requirements?.LightRequirements?.MaximumLight != Models.LightType.Unknown ? source.Requirements?.LightRequirements?.MaximumLight.ToString() : null,
            MinimumWater = source.Requirements?.WaterRequirements?.MinimumWater != Models.WaterType.Unknown ? source.Requirements?.WaterRequirements?.MinimumWater.ToString() : null,
            MaximumWater = source.Requirements?.WaterRequirements?.MaximumWater != Models.WaterType.Unknown ? source.Requirements?.WaterRequirements?.MaximumWater.ToString() : null,
            MinimumZone = source.Requirements?.ZoneRequirements?.MinimumZone?.Letter + source.Requirements?.ZoneRequirements?.MinimumZone?.Number ?? "",
            MaximumZone = source.Requirements?.ZoneRequirements?.MaximumZone?.Letter + source.Requirements?.ZoneRequirements?.MaximumZone?.Number ?? "",
            StratificationStages = source.Requirements?.StratificationStages != null ? JsonConvert.SerializeObject(source.Requirements.StratificationStages) : null,
            Preferred = source.Preferred,
            TaxonId = source.Taxon?.TaxonId,
            CreatedBy = source.CreatedBy,
            DateCreated = source.DateCreated ?? DateTime.UtcNow,
            DateModified = source.DateModified
        };
    }
}
