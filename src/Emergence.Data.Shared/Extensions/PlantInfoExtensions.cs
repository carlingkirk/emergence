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
                    Unit = source.MinimumHeight.HasValue || source.MaximumHeight.HasValue ? DistanceUnit.Feet : DistanceUnit.Unknown
                },
                Spread = new Models.Spread
                {
                    MinimumSpread = source.MinimumSpread,
                    MaximumSpread = source.MaximumSpread,
                    Unit = source.MinimumSpread.HasValue || source.MaximumSpread.HasValue ? DistanceUnit.Feet : DistanceUnit.Unknown
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
                    ZoneRequirements = (source.MinimumZone != null || source.MaximumZone != null) ? new Models.ZoneRequirements
                    {
                        MinimumZone = source.MinimumZone != null ? new Models.Zone
                        {
                            Id = source.MinimumZone.Id,
                            Name = source.MinimumZone.Name,
                            Notes = source.MinimumZone.Notes
                        } : null,
                        MaximumZone = source.MaximumZone != null ? new Models.Zone
                        {
                            Id = source.MaximumZone.Id,
                            Name = source.MaximumZone.Name,
                            Notes = source.MaximumZone.Notes
                        } : null
                    } : null
                },
                Taxon = source.Taxon != null ? source.Taxon.AsModel() : source.TaxonId.HasValue ? new Models.Taxon { TaxonId = source.TaxonId.Value } : null,
                Preferred = source.Preferred,
                Visibility = source.Visibility,
                UserId = source.UserId,
                User = source.User?.AsSummaryModel(),
                CreatedBy = source.CreatedBy,
                ModifiedBy = source.ModifiedBy,
                DateCreated = source.DateCreated,
                DateModified = source.DateModified
            };
        }

        public static PlantInfo AsStore(this Models.PlantInfo source)
        {
            if (source.Height?.MinimumHeight != null && source.Height.MaximumHeight != null && source.Height.MinimumHeight >= source.Height.MaximumHeight)
            {
                var minHeight = source.Height.MinimumHeight;
                var maxHeight = source.Height.MaximumHeight;

                source.Height.MinimumHeight = maxHeight;
                source.Height.MaximumHeight = minHeight;
            }

            if (source.Spread?.MinimumSpread != null && source.Spread?.MaximumSpread != null && source.Spread.MinimumSpread >= source.Spread.MaximumSpread)
            {
                var minSpread = source.Spread.MinimumSpread;
                var maxSpread = source.Spread.MaximumSpread;

                source.Spread.MinimumSpread = maxSpread;
                source.Spread.MaximumSpread = minSpread;
            }

            if (source.Requirements?.WaterRequirements?.MinimumWater != WaterType.Unknown && source.Requirements?.WaterRequirements?.MaximumWater != WaterType.Unknown &&
                source.Requirements.WaterRequirements.MinimumWater >= source.Requirements.WaterRequirements.MaximumWater)
            {
                var minWater = source.Requirements.WaterRequirements.MinimumWater;
                var maxWater = source.Requirements.WaterRequirements.MaximumWater;

                source.Requirements.WaterRequirements.MinimumWater = maxWater;
                source.Requirements.WaterRequirements.MaximumWater = minWater;
            }

            if (source.Requirements?.LightRequirements?.MinimumLight != LightType.Unknown && source.Requirements?.LightRequirements?.MaximumLight != LightType.Unknown &&
                source.Requirements.LightRequirements.MinimumLight >= source.Requirements.LightRequirements.MaximumLight)
            {
                var minLight = source.Requirements.LightRequirements.MinimumLight;
                var maxLight = source.Requirements.LightRequirements.MaximumLight;

                source.Requirements.LightRequirements.MinimumLight = maxLight;
                source.Requirements.LightRequirements.MaximumLight = minLight;
            }

            if (source.Requirements?.ZoneRequirements?.MinimumZone != null && source.Requirements?.ZoneRequirements?.MaximumZone != null &&
                source.Requirements.ZoneRequirements.MinimumZone.Id >= source.Requirements.ZoneRequirements.MaximumZone.Id)
            {
                var minZone = source.Requirements.ZoneRequirements.MinimumZone;
                var maxZone = source.Requirements.ZoneRequirements.MaximumZone;

                source.Requirements.ZoneRequirements.MinimumZone = new Models.Zone { Id = maxZone.Id };
                source.Requirements.ZoneRequirements.MaximumZone = new Models.Zone { Id = minZone.Id };
            }

            return new PlantInfo
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
                HeightUnit = source.Height?.Unit != DistanceUnit.Unknown ? DistanceUnit.Feet.ToString() : null,
                MinimumSpread = source.Spread?.MinimumSpread,
                MaximumSpread = source.Spread?.MaximumSpread,
                SpreadUnit = source.Spread?.Unit != DistanceUnit.Unknown ? DistanceUnit.Feet.ToString() : null,
                MinimumLight = source.Requirements?.LightRequirements?.MinimumLight != LightType.Unknown ? source.Requirements?.LightRequirements?.MinimumLight.ToString() : null,
                MaximumLight = source.Requirements?.LightRequirements?.MaximumLight != LightType.Unknown ? source.Requirements?.LightRequirements?.MaximumLight.ToString() : null,
                MinimumWater = source.Requirements?.WaterRequirements?.MinimumWater != WaterType.Unknown ? source.Requirements?.WaterRequirements?.MinimumWater.ToString() : null,
                MaximumWater = source.Requirements?.WaterRequirements?.MaximumWater != WaterType.Unknown ? source.Requirements?.WaterRequirements?.MaximumWater.ToString() : null,
                MinimumZoneId = source.Requirements?.ZoneRequirements?.MinimumZone?.Id,
                MaximumZoneId = source.Requirements?.ZoneRequirements?.MaximumZone?.Id,
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

        public static Search.Models.PlantInfo AsSearchModel(this PlantInfo source, IEnumerable<PlantLocation> plantLocations, IEnumerable<Synonym> synonyms) =>
            new Search.Models.PlantInfo
            {
                Id = source.Id,
                CommonName = source.CommonName,
                ScientificName = source.ScientificName,
                Origin = source.Origin?.AsSearchModel(),
                Lifeform = source.Lifeform.AsSearchModel(),
                PlantLocations = plantLocations?.Select(pl => pl.AsSearchModel()),
                Synonyms = synonyms?.Select(s => s.AsSearchModel()),
                MinimumBloomTime = source.MinimumBloomTime,
                MaximumBloomTime = source.MaximumBloomTime,
                MinimumHeight = source.MinimumHeight,
                MaximumHeight = source.MaximumHeight,
                HeightUnit = source.MinimumHeight.HasValue || source.MaximumHeight.HasValue ? DistanceUnit.Feet : null,
                MinimumSpread = source.MinimumSpread,
                MaximumSpread = source.MaximumSpread,
                SpreadUnit = source.MinimumSpread.HasValue || source.MaximumSpread.HasValue ? DistanceUnit.Feet : null,
                MinimumLight = !string.IsNullOrEmpty(source.MinimumLight) ? Enum.Parse<LightType>(source.MinimumLight) : null,
                MaximumLight = !string.IsNullOrEmpty(source.MaximumLight) ? Enum.Parse<LightType>(source.MaximumLight) : null,
                MinimumWater = !string.IsNullOrEmpty(source.MinimumWater) ? Enum.Parse<WaterType>(source.MinimumWater) : null,
                MaximumWater = !string.IsNullOrEmpty(source.MaximumWater) ? Enum.Parse<WaterType>(source.MaximumWater) : null,
                MinimumZone = source.MinimumZone?.AsSearchModel(),
                MaximumZone = source.MaximumZone?.AsSearchModel(),
                StratificationStages = source.StratificationStages != null ? JsonConvert.DeserializeObject<List<Search.Models.StratificationStage>>(source.StratificationStages) : null,
                Preferred = source.Preferred,
                Taxon = source.Taxon?.AsSearchModel(),
                Visibility = source.Visibility,
                User = source.User?.AsSearchModel(),
                CreatedBy = source.CreatedBy,
                ModifiedBy = source.ModifiedBy,
                DateCreated = source.DateCreated,
                DateModified = source.DateModified
            };
    }
}
