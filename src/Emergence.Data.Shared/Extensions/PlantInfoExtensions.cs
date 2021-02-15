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

            List<WildlifeEffect> wildlifeEffects = null;
            if (!string.IsNullOrEmpty(source.WildlifeEffects))
            {
                wildlifeEffects = JsonConvert.DeserializeObject<List<WildlifeEffect>>(source.WildlifeEffects);
            }

            List<SoilType> soilTypes = null;
            if (!string.IsNullOrEmpty(source.SoilTypes))
            {
                soilTypes = JsonConvert.DeserializeObject<List<SoilType>>(source.SoilTypes);
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
                Locations = source.PlantLocations?.Select(pl => pl.AsModel()),
                Preferred = source.Preferred,
                Visibility = source.Visibility,
                WildlifeEffects = wildlifeEffects,
                SoilTypes = soilTypes,
                Notes = source.Notes,
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

            if (source.Requirements?.WaterRequirements != null && source.Requirements?.WaterRequirements?.MinimumWater != WaterType.Unknown &&
                source.Requirements?.WaterRequirements?.MaximumWater != WaterType.Unknown &&
                source.Requirements.WaterRequirements.MinimumWater >= source.Requirements.WaterRequirements.MaximumWater)
            {
                var minWater = source.Requirements.WaterRequirements.MinimumWater;
                var maxWater = source.Requirements.WaterRequirements.MaximumWater;

                source.Requirements.WaterRequirements.MinimumWater = maxWater;
                source.Requirements.WaterRequirements.MaximumWater = minWater;
            }

            if (source.Requirements?.LightRequirements != null && source.Requirements?.LightRequirements?.MinimumLight != LightType.Unknown &&
                source.Requirements?.LightRequirements?.MaximumLight != LightType.Unknown &&
                source.Requirements.LightRequirements.MinimumLight >= source.Requirements.LightRequirements.MaximumLight)
            {
                var minLight = source.Requirements.LightRequirements.MinimumLight;
                var maxLight = source.Requirements.LightRequirements.MaximumLight;

                source.Requirements.LightRequirements.MinimumLight = maxLight;
                source.Requirements.LightRequirements.MaximumLight = minLight;
            }

            if (source.Requirements?.ZoneRequirements != null && source.Requirements?.ZoneRequirements?.MinimumZone != null && source.Requirements?.ZoneRequirements?.MaximumZone != null &&
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
                WildlifeEffects = source.WildlifeEffects != null ? JsonConvert.SerializeObject(source.WildlifeEffects) : null,
                SoilTypes = source.SoilTypes != null ? JsonConvert.SerializeObject(source.SoilTypes) : null,
                Notes = source.Notes,
                Visibility = source.Visibility,
                UserId = source.UserId,
                CreatedBy = source.CreatedBy,
                ModifiedBy = source.ModifiedBy,
                DateCreated = source.DateCreated ?? DateTime.UtcNow,
                DateModified = source.DateModified
            };
        }

        public static Search.Models.PlantInfo AsSearchModel(this PlantInfo source, IEnumerable<PlantLocation> plantLocations, IEnumerable<Synonym> synonyms)
        {
            var plantLocationSearch = plantLocations?.Select(pl => pl.AsSearchModel()) ?? source.PlantLocations?.Select(pl => pl.AsSearchModel());

            var bloomTimes = new List<Month>();
            if (source.MinimumBloomTime > 0 && source.MaximumBloomTime > 0)
            {
                for (var i = source.MinimumBloomTime; i <= source.MaximumBloomTime; i++)
                {
                    bloomTimes.Add((Month)i);
                }
            }
            else
            {
                if (source.MinimumBloomTime > 0)
                {
                    bloomTimes.Add((Month)source.MinimumBloomTime);
                }
                if (source.MaximumBloomTime > 0 && source.MinimumBloomTime != source.MaximumBloomTime)
                {
                    bloomTimes.Add((Month)source.MaximumBloomTime);
                }
            }

            var waterTypes = new List<WaterType>();

            if (!string.IsNullOrEmpty(source.MinimumWater) && !string.IsNullOrEmpty(source.MaximumWater))
            {
                var minWater = Enum.Parse<WaterType>(source.MinimumWater);
                var maxWater = Enum.Parse<WaterType>(source.MaximumWater);
                for (var i = minWater; i <= maxWater; i++)
                {
                    waterTypes.Add(i);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(source.MinimumWater))
                {
                    var minWater = Enum.Parse<WaterType>(source.MinimumWater);
                    waterTypes.Add(minWater);
                }
                if (!string.IsNullOrEmpty(source.MaximumWater) && source.MinimumWater != source.MaximumWater)
                {
                    var maxWater = Enum.Parse<WaterType>(source.MaximumWater);
                    waterTypes.Add(maxWater);
                }
            }

            var lightTypes = new List<LightType>();

            if (!string.IsNullOrEmpty(source.MinimumLight) && !string.IsNullOrEmpty(source.MaximumLight))
            {
                var minLight = Enum.Parse<LightType>(source.MinimumLight);
                var maxLight = Enum.Parse<LightType>(source.MaximumLight);
                for (var i = minLight; i <= maxLight; i++)
                {
                    lightTypes.Add(i);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(source.MinimumLight))
                {
                    var minLight = Enum.Parse<LightType>(source.MinimumLight);
                    lightTypes.Add(minLight);
                }
                if (!string.IsNullOrEmpty(source.MaximumLight) && source.MinimumLight != source.MaximumLight)
                {
                    var maxLight = Enum.Parse<LightType>(source.MaximumLight);
                    lightTypes.Add(maxLight);
                }
            }

            var zones = new List<Search.Models.Zone>();
            var allZones = ZoneHelper.GetZones();
            if (source.MinimumZone != null && source.MaximumZone != null)
            {
                for (var i = source.MinimumZone.Id; i <= source.MaximumZone.Id; i++)
                {
                    zones.Add(allZones.First(z => z.Id == i).AsSearchModel());
                }
            }
            else
            {
                if (source.MinimumZone != null)
                {
                    zones.Add(source.MinimumZone.AsSearchModel());
                }
                if (source.MaximumZone != null && source.MinimumZone.Id != source.MaximumZone.Id)
                {
                    zones.Add(source.MaximumZone.AsSearchModel());
                }
            }

            return new Search.Models.PlantInfo
            {
                Id = source.Id,
                CommonName = source.CommonName,
                ScientificName = source.ScientificName,
                Origin = source.Origin?.AsSearchModel(),
                Lifeform = source.Lifeform.AsSearchModel(),
                PlantLocations = plantLocationSearch,
                Synonyms = synonyms?.Select(s => s.AsSearchModel()),
                MinimumBloomTime = source.MinimumBloomTime,
                MaximumBloomTime = source.MaximumBloomTime,
                BloomTimes = bloomTimes.Any() ? bloomTimes : null,
                MinimumHeight = source.MinimumHeight,
                MaximumHeight = source.MaximumHeight,
                HeightUnit = source.MinimumHeight.HasValue || source.MaximumHeight.HasValue ? DistanceUnit.Feet : null,
                MinimumSpread = source.MinimumSpread,
                MaximumSpread = source.MaximumSpread,
                SpreadUnit = source.MinimumSpread.HasValue || source.MaximumSpread.HasValue ? DistanceUnit.Feet : null,
                MinimumLight = !string.IsNullOrEmpty(source.MinimumLight) ? Enum.Parse<LightType>(source.MinimumLight) : null,
                MaximumLight = !string.IsNullOrEmpty(source.MaximumLight) ? Enum.Parse<LightType>(source.MaximumLight) : null,
                LightTypes = lightTypes.Any() ? lightTypes : null,
                MinimumWater = !string.IsNullOrEmpty(source.MinimumWater) ? Enum.Parse<WaterType>(source.MinimumWater) : null,
                MaximumWater = !string.IsNullOrEmpty(source.MaximumWater) ? Enum.Parse<WaterType>(source.MaximumWater) : null,
                WaterTypes = waterTypes.Any() ? waterTypes : null,
                MinimumZone = source.MinimumZone?.AsSearchModel(),
                MaximumZone = source.MaximumZone?.AsSearchModel(),
                Zones = zones.Any() ? zones : null,
                StratificationStages = source.StratificationStages != null ? JsonConvert.DeserializeObject<List<Search.Models.StratificationStage>>(source.StratificationStages) : null,
                Preferred = source.Preferred,
                Taxon = source.Taxon?.AsSearchModel(),
                WildlifeEffects = source.WildlifeEffects != null ? JsonConvert.DeserializeObject<List<WildlifeEffect>>(source.WildlifeEffects) : null,
                SoilTypes = source.SoilTypes != null ? JsonConvert.DeserializeObject<List<SoilType>>(source.SoilTypes) : null,
                Notes = source.Notes,
                Visibility = source.Visibility,
                User = source.User?.AsSearchModel(),
                CreatedBy = source.CreatedBy,
                ModifiedBy = source.ModifiedBy,
                DateCreated = source.DateCreated,
                DateModified = source.DateModified
            };
        }

        public static Models.PlantInfo Clone(this Models.PlantInfo plantInfo, string userId) => new Models.PlantInfo
        {
            PlantInfoId = 0,
            Lifeform = plantInfo.Lifeform,
            LifeformId = plantInfo.LifeformId,
            ScientificName = plantInfo.ScientificName,
            CommonName = plantInfo.CommonName,
            BloomTime = plantInfo.BloomTime,
            Height = plantInfo.Height,
            Spread = plantInfo.Spread,
            Requirements = plantInfo.Requirements,
            WildlifeEffects = plantInfo.WildlifeEffects,
            SoilTypes = plantInfo.SoilTypes,
            Notes = plantInfo.Notes,
            Visibility = Visibility.Inherit,
            UserId = null,
            CreatedBy = userId,
            DateCreated = DateTime.UtcNow,
            Taxon = plantInfo.Taxon,
            Origin = null,
            Locations = plantInfo.Locations,
            Photos = null,
            User = null
        };
    }
}
