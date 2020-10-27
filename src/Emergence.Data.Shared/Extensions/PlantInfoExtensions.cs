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
                string minimumZoneLetter = null;
                if (source.MinimumZone.Length == 2)
                {
                    minimumZoneLetter = source.MinimumZone.Last().ToString();
                }

                minimumZone = new Models.Zone
                {
                    Number = minimumZoneNumber,
                    Letter = minimumZoneLetter
                };
            }

            if (!string.IsNullOrEmpty(source.MaximumZone))
            {
                int.TryParse(source.MaximumZone.First().ToString(), out var maximumZoneNumber);
                string maximumZoneLetter = null;
                if (source.MaximumZone.Length == 2)
                {
                    maximumZoneLetter = source.MaximumZone.Last().ToString();
                }

                maximumZone = new Models.Zone
                {
                    Number = maximumZoneNumber,
                    Letter = maximumZoneLetter
                };
            }

            return new Models.PlantInfo
            {
                PlantInfoId = source.Id,
                CommonName = source.CommonName,
                ScientificName = source.ScientificName,
                LifeformId = source.LifeformId,
                Lifeform = source.Lifeform != null ? source.Lifeform.AsModel() : new Models.Lifeform { LifeformId = source.LifeformId },
                Origin = source.Origin != null ? source.Origin.AsModel() : source.OriginId.HasValue ? new Models.Origin { OriginId = source.OriginId.Value } : null,
                BloomTime = new Models.BloomTime
                {
                    MinimumBloomTime = source.MinimumBloomTime.HasValue ? (Month)source.MinimumBloomTime.Value : Month.Unknown,
                    MaximumBloomTime = source.MaximumBloomTime.HasValue ? (Month)source.MaximumBloomTime.Value : Month.Unknown
                },
                Height = new Models.Height
                {
                    MinimumHeight = source.MinimumHeight,
                    MaximumHeight = source.MaximumHeight,
                    Unit = !string.IsNullOrEmpty(source.HeightUnit) ? Enum.Parse<DistanceUnit>(source.HeightUnit) : DistanceUnit.Unknown
                },
                Spread = new Models.Spread
                {
                    MinimumSpread = source.MinimumSpread,
                    MaximumSpread = source.MaximumSpread,
                    Unit = !string.IsNullOrEmpty(source.SpreadUnit) ? Enum.Parse<DistanceUnit>(source.SpreadUnit) : DistanceUnit.Unknown
                },
                Requirements = new Models.Requirements
                {
                    LightRequirements = new Models.LightRequirements
                    {
                        MinimumLight = !string.IsNullOrEmpty(source.MinimumLight) ? Enum.Parse<LightType>(source.MinimumLight) : LightType.Unknown,
                        MaximumLight = !string.IsNullOrEmpty(source.MaximumLight) ? Enum.Parse<LightType>(source.MaximumLight) : LightType.Unknown
                    },
                    WaterRequirements = new Models.WaterRequirements
                    {
                        MinimumWater = !string.IsNullOrEmpty(source.MinimumWater) ? Enum.Parse<WaterType>(source.MinimumWater) : WaterType.Unknown,
                        MaximumWater = !string.IsNullOrEmpty(source.MaximumWater) ? Enum.Parse<WaterType>(source.MaximumWater) : WaterType.Unknown,
                    },
                    SoilRequirements = null,
                    StratificationStages = stratificationStages,
                    ZoneRequirements = new Models.ZoneRequirements
                    {
                        MinimumZone = minimumZone,
                        MaximumZone = maximumZone
                    }
                },
                Taxon = source.Taxon != null ? source.Taxon.AsModel() : source.TaxonId.HasValue ? new Models.Taxon { TaxonId = source.TaxonId.Value } : null,
                Preferred = source.Preferred,
                Visibility = source.Visibility,
                UserId = source.UserId,
                CreatedBy = source.CreatedBy,
                ModifiedBy = source.ModifiedBy,
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
            HeightUnit = source.Height?.Unit != DistanceUnit.Unknown ? source.Height?.Unit.ToString() : null,
            MinimumSpread = source.Spread?.MinimumSpread,
            MaximumSpread = source.Spread?.MaximumSpread,
            SpreadUnit = source.Spread?.Unit != DistanceUnit.Unknown ? source.Spread?.Unit.ToString() : null,
            MinimumLight = source.Requirements?.LightRequirements?.MinimumLight != LightType.Unknown ? source.Requirements?.LightRequirements?.MinimumLight.ToString() : null,
            MaximumLight = source.Requirements?.LightRequirements?.MaximumLight != LightType.Unknown ? source.Requirements?.LightRequirements?.MaximumLight.ToString() : null,
            MinimumWater = source.Requirements?.WaterRequirements?.MinimumWater != WaterType.Unknown ? source.Requirements?.WaterRequirements?.MinimumWater.ToString() : null,
            MaximumWater = source.Requirements?.WaterRequirements?.MaximumWater != WaterType.Unknown ? source.Requirements?.WaterRequirements?.MaximumWater.ToString() : null,
            MinimumZone = source.Requirements?.ZoneRequirements?.MinimumZone != null
                ? source.Requirements?.ZoneRequirements?.MinimumZone?.Number + source.Requirements?.ZoneRequirements?.MinimumZone?.Letter ?? ""
                : null,
            MaximumZone = source.Requirements?.ZoneRequirements?.MaximumZone != null
                ? source.Requirements?.ZoneRequirements?.MaximumZone?.Number + source.Requirements?.ZoneRequirements?.MaximumZone?.Letter ?? ""
                : null,
            StratificationStages = source.Requirements?.StratificationStages != null ? JsonConvert.SerializeObject(source.Requirements.StratificationStages) : null,
            Preferred = source.Preferred,
            TaxonId = source.Taxon?.TaxonId,
            Visibility = source.Visibility,
            UserId = source.UserId,
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated ?? DateTime.UtcNow,
            DateModified = source.DateModified
        };
    }
}
