using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.External.ITIS;
using Emergence.Data.External.USDA;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Emergence.Service;
using Emergence.Service.Extensions;
using Emergence.Test.Mocks;
using Emergence.Transform;
using FluentAssertions;
using Moq;
using Xunit;
using Stores = Emergence.Data.Shared.Stores;

namespace Emergence.Test.Emergence.Transform
{
    public class ITISTests
    {
        [Fact]
        public void TestITISPlantInfoTransformer()
        {
            var transformer = new ITISPlantInfoTransformer();

            var itisData = ITISPlantInfoData();
            var result = new List<PlantInfo>();
            itisData.ForEach(i => result.AddRange(transformer.Transform(new List<TaxonomicUnit> { i })));

            result.Count(p => p.ScientificName == "Glandularia quadrangulata").Should().Be(1);
            result.Count(p => p.Taxon.Subfamily == null).Should().Be(6);
            result.Count(p => p.Taxon.Species == "cremersii").Should().Be(2);
            result.Count(p => p.Taxon.Form == "viridifolia").Should().Be(2);
            result.Count(p => p.Taxon.Subspecies == "purpurea").Should().Be(1);
            result.Count(p => p.Taxon.Variety == "graminea").Should().Be(1);
            result.Where(p => p.Locations != null).SelectMany(p => p.Locations).Count().Should().Be(3);
            result.SelectMany(p => p.Locations).DistinctBy(l => l.Location.Region).Count().Should().Be(1);
            result.Select(p => p.Origin).Count().Should().Be(6);
        }

        [Fact]
        public async Task TestITISPlantInfoProcessor()
        {
            var transformer = new ITISPlantInfoTransformer();
            var itisData = ITISPlantInfoData();
            var plantInfos = new List<PlantInfo>();
            itisData.ForEach(i => plantInfos.AddRange(transformer.Transform(new List<TaxonomicUnit> { i })));

            var originRepository = RepositoryMocks.GetStandardMockOriginRepository(new List<Stores.Origin>());
            var locationRepository = RepositoryMocks.GetStandardMockLocationRepository(new List<Stores.Location>());
            var lifeformRepository = RepositoryMocks.GetStandardMockLifeformRepository(new List<Stores.Lifeform>());
            var plantInfoRepository = RepositoryMocks.GetStandardMockPlantInfoRepository(new List<Stores.PlantInfo>());
            var plantLocationRepository = RepositoryMocks.GetStandardMockPlantLocationRepository(new List<Stores.PlantLocation>());
            var synonymRepository = RepositoryMocks.GetStandardMockSynonymRepository();
            var taxonRepository = RepositoryMocks.GetStandardMockTaxonRepository(new List<Stores.Taxon>());
            var plantSynonymRepository = new Mock<IRepository<Stores.PlantSynonym>>();
            var plantInfoIndex = SearchMocks.GetStandardMockPlantInfoIndex();

            var locationService = new LocationService(locationRepository.Object);
            var originService = new OriginService(originRepository.Object, locationService);
            var lifeformService = new LifeformService(lifeformRepository.Object, plantSynonymRepository.Object);
            var plantInfoService = new PlantInfoService(plantInfoRepository.Object, plantLocationRepository.Object, plantInfoIndex.Object);
            var synonymService = new SynonymService(synonymRepository.Object);
            var taxonService = new TaxonService(taxonRepository.Object, synonymService);

            var processor = new PlantInfoProcessor(lifeformService, originService, plantInfoService, taxonService, locationService);

            await processor.InitializeOrigin(transformer.Origin);
            await processor.InitializeLifeforms();
            await processor.InitializeTaxons();

            var result = await processor.Process(plantInfos);

            result.Count(p => p.ScientificName == "Glandularia quadrangulata").Should().Be(1);
            result.Count(p => p.Taxon.Subfamily == null).Should().Be(5);
            result.Count(p => p.Taxon.Species == "cremersii").Should().Be(1);
            result.Count(p => p.Taxon.Form == "viridifolia").Should().Be(1);
            result.Count(p => p.Taxon.Subspecies == "purpurea").Should().Be(1);
            result.Count(p => p.Taxon.Variety == "graminea").Should().Be(1);
            result.Where(p => p.Locations != null).SelectMany(p => p.Locations).Count().Should().Be(3);
            result.Where(p => p.Locations != null).SelectMany(p => p.Locations).Count(l => l.Status == LocationStatus.Native).Should().Be(3);
            result.Select(p => p.Origin).DistinctBy(o => o.OriginId).Count().Should().Be(5);
        }

        [Fact]
        public void TestITISSynonymTransformer()
        {
            var transformer = new ITISSynonymTransformer();

            var itisData = ITISSynonymData();
            var result = new List<Synonym>();
            itisData.ForEach(i => result.Add(transformer.Transform(i)));

            result.Count.Should().Be(16);
            result.Count(s => s.Taxon.Kingdom == "Plantae").Should().Be(1);
            result.Count(s => s.Language == "English").Should().Be(10);
            result.Count(s => s.DateUpdated.Year == 2003).Should().Be(3);
            result.Count(s => s.Rank == "Variety").Should().Be(2);
            result.Count(s => s.Taxon.Variety != null).Should().Be(2);
            result.Count(s => s.Taxon.Variety == "paludicola").Should().Be(1);
            result.Select(s => s.Origin).Count().Should().Be(16);
        }

        [Fact]
        public async Task TestITISSynonymProcessor()
        {
            var transformer = new ITISSynonymTransformer();

            var itisData = ITISSynonymData();
            var synonyms = new List<Synonym>();
            itisData.ForEach(i => synonyms.Add(transformer.Transform(i)));

            var originRepository = RepositoryMocks.GetStandardMockOriginRepository(new List<Stores.Origin>());
            var locationRepository = RepositoryMocks.GetStandardMockLocationRepository(new List<Stores.Location>());
            var synonymRepository = RepositoryMocks.GetStandardMockSynonymRepository(new List<Stores.Synonym>());
            var taxonRepository = RepositoryMocks.GetStandardMockTaxonRepository(new List<Stores.Taxon>());

            var locationService = new LocationService(locationRepository.Object);
            var originService = new OriginService(originRepository.Object, locationService);
            var synonymService = new SynonymService(synonymRepository.Object);
            var taxonService = new TaxonService(taxonRepository.Object, synonymService);

            var processor = new SynonymProcessor(synonymService, originService, taxonService);

            await processor.InitializeOrigin(transformer.Origin);
            await processor.InitializeTaxons();

            var result = await processor.Process(synonyms);

            result.Count().Should().Be(16);
            result.Count(s => s.Taxon.Kingdom == "Plantae").Should().Be(1);
            result.Count(s => s.Language == "English").Should().Be(10);
            result.Count(s => s.DateUpdated.Year == 2003).Should().Be(3);
            result.Count(s => s.Rank == "Variety").Should().Be(2);
            result.Count(s => s.Taxon.Variety != null).Should().Be(2);
            result.Count(s => s.Taxon.Variety == "paludicola").Should().Be(1);
            result.Select(s => s.Origin).Count().Should().Be(16);
        }

        private static List<TaxonomicUnit> ITISPlantInfoData() => new List<TaxonomicUnit>
        {
            // Variety
            new TaxonomicUnit
            {
                Tsn = "1115929",
                Kingdom = "Plantae",
                Subkingdom = "Viridiplantae",
                Infrakingdom = "Streptophyta",
                Superdivision = "Embryophyta",
                Division = "Marchantiophyta",
                Subdivision = "",
                Class = "Jungermanniopsida",
                Subclass = "Jungermanniidae",
                Superorder = "",
                Order = "Jungermanniales",
                Suborder = "Cephaloziineae",
                Family = "Scapaniaceae",
                Subfamily = "",
                Genus = "Scapania",
                Subgenus = "Scapania (Scapania)",
                Species = "Scapania glaucocephala",
                Subspecies = "",
                Variety = "Scapania glaucocephala var. glaucocephala",
                Form = "",
                Author = "(Taylor) Austin",
                SourceId = "1196",
                SourceName = "ELPT (Marchantiophyta & Anthocerotophyta)",
                SourceDescription = "Early Land Plants Today. Söderström L, Hagborg A, von Konrat M (eds.) The Early Land Plants Today (ELPT) project aims to combine existing data on nomenclature, taxonomy and distribution for liverworts and hornworts. The project is jointly coordinated by Prof. Lars Söderström (Norwegian University of Science and Technology, Trondheim, Norway) and Anders Hagborg and Dr. Matt von Konrat (The Field Museum of Natural History, Illinois). http://www.elpt.org",
                SourceType = "database  ",
                Country = "United States",
                Region = "North America",
                LocationStatus = "Native",
            },
            // Form and subspecies
            new TaxonomicUnit
            {
                Tsn = "896959",
                Kingdom = "Plantae",
                Subkingdom = "Viridiplantae",
                Infrakingdom = "Streptophyta",
                Superdivision = "Embryophyta",
                Division = "Tracheophyta",
                Subdivision = "Spermatophytina",
                Class = "Magnoliopsida",
                Subclass = "",
                Superorder = "Asteranae",
                Order = "Ericales",
                Suborder = "",
                Family = "Sarraceniaceae",
                Subfamily = "",
                Genus = "Sarracenia",
                Subgenus = "",
                Species = "Sarracenia purpurea",
                Subspecies = "Sarracenia purpurea ssp. purpurea",
                Variety = "",
                Form = "Sarracenia purpurea f. heterophylla",
                Author = "(Eaton) Fernald",
                SourceId = "207",
                SourceName = "T. Lawrence Mellichamp",
                SourceDescription = "Professor of Botany and Horticulture, Botanical Gardens, UNC Charlotte, 9201 University City Blvd., Charlotte, North Carolina, USA 28223",
                SourceType = "Expert",
                Country = "",
                Region = "",
                LocationStatus = "",
            },
            // Form
            new TaxonomicUnit
            {
                Tsn = "834072",
                Kingdom = "Plantae",
                Subkingdom = "Viridiplantae",
                Infrakingdom = "Streptophyta",
                Superdivision = "Embryophyta",
                Division = "Tracheophyta",
                Subdivision = "Spermatophytina",
                Class = "Magnoliopsida",
                Subclass = "",
                Superorder = "Asteranae",
                Order = "Lamiales",
                Suborder = "",
                Family = "Verbenaceae",
                Subfamily = "",
                Genus = "Glandularia",
                Subgenus = "",
                Species = "Glandularia quadrangulata",
                Subspecies = "",
                Variety = "",
                Form = "Glandularia quadrangulata f. albida",
                Author = "(Moldenke) B.L. Turner",
                SourceId = "591",
                SourceName = "Lamiales of North America Update",
                SourceDescription = "Updated for ITIS by the Flora of North America Expertise Network, in connection with an update for USDA PLANTS (2007-2010)",
                SourceType = "database",
                Country = "United States",
                Region = "North America",
                LocationStatus = "Native",
            },
            // Variety and Form
            new TaxonomicUnit
            {
                Tsn = "1034018",
                Kingdom = "Plantae",
                Subkingdom = "Viridiplantae",
                Infrakingdom = "Streptophyta",
                Superdivision = "Embryophyta",
                Division = "Tracheophyta",
                Subdivision = "Spermatophytina",
                Class = "Magnoliopsida",
                Subclass = "",
                Superorder = "Rosanae",
                Order = "Malpighiales",
                Suborder = "",
                Family = "Euphorbiaceae",
                Subfamily = "",
                Genus = "Euphorbia",
                Subgenus = "",
                Species = "Euphorbia graminea",
                Subspecies = "",
                Variety = "Euphorbia graminea var. graminea",
                Form = "Euphorbia graminea f. foliosa",
                Author = "McVaugh",
                SourceId = "233",
                SourceName = "Paul E. Berry, PhD",
                SourceDescription = "Director, University of Michigan Herbarium",
                SourceType = "Expert",
                Country = "Mexico",
                Region = "North America",
                LocationStatus = "Native",
            },
            new TaxonomicUnit
            {
                Tsn = "845711",
                Kingdom = "Plantae",
                Subkingdom = "Plantae",
                Infrakingdom = "Viridiplantae",
                Superdivision = "Streptophyta",
                Division = "Embryophyta",
                Subdivision = "Spermatophytina",
                Class = "Magnoliopsida",
                Subclass = "",
                Superorder = "Rosanae",
                Order = "Malpighiales",
                Suborder = "",
                Family = "Euphorbiaceae",
                Subfamily = "",
                Genus = "Euphorbia",
                Subgenus = "",
                Species = "Euphorbia cremersii",
                Subspecies = "",
                Variety = "Euphorbia cremersii var. cremersii",
                Form = "Euphorbia cremersii f. viridifolia",
                Author = "Rauh",
                SourceId = "767",
                SourceName = "Checklist of CITES Species Part 2 History of CITES listings",
                SourceDescription = "UNEP-WCMC (Comps.) 2011. Checklist of CITES species (CD-ROM). CITES Secretariat, Geneva, Switzerland, and UNEP-WCMC, Cambridge, United Kingdom. ISBN 2-88323-030-7. Available online at http://www.cites.org/eng/resources/pub/checklist11/index.html or from CITES Secretariat, Chemin des Anémones, 1219 Châtelaine, Genève, Switzerland",
                SourceType = "CD-ROM",
                Country = "",
                Region = "",
                LocationStatus = "",
            },
            // Duplicate
            new TaxonomicUnit
            {
                Tsn = "845711",
                Kingdom = "Plantae",
                Subkingdom = "Plantae",
                Infrakingdom = "Viridiplantae",
                Superdivision = "Streptophyta",
                Division = "Embryophyta",
                Subdivision = "Spermatophytina",
                Class = "Magnoliopsida",
                Subclass = "",
                Superorder = "Rosanae",
                Order = "Malpighiales",
                Suborder = "",
                Family = "Euphorbiaceae",
                Subfamily = "",
                Genus = "Euphorbia",
                Subgenus = "",
                Species = "Euphorbia cremersii",
                Subspecies = "",
                Variety = "Euphorbia cremersii var. cremersii",
                Form = "Euphorbia cremersii f. viridifolia",
                Author = "Rauh",
                SourceId = "767",
                SourceName = "Checklist of CITES Species Part 2 History of CITES listings",
                SourceDescription = "UNEP-WCMC (Comps.) 2011. Checklist of CITES species (CD-ROM). CITES Secretariat, Geneva, Switzerland, and UNEP-WCMC, Cambridge, United Kingdom. ISBN 2-88323-030-7. Available online at http://www.cites.org/eng/resources/pub/checklist11/index.html or from CITES Secretariat, Chemin des Anémones, 1219 Châtelaine, Genève, Switzerland",
                SourceType = "CD-ROM",
                Country = "",
                Region = "",
                LocationStatus = "",
            }
        };

        private static List<Vernacular> ITISSynonymData() => new List<Vernacular>
        {
            new Vernacular
            {
                Tsn = "202422",
                Rank = "Kingdom",
                Kingdom = "Plantae",
                Name = "plantes",
                DateUpdated = DateTime.Parse("2003-05-21 00:00:00"),
                Language = "French",
                Taxon = "Plantae"
            },
            new Vernacular
            {
                Tsn = "846492",
                Rank = "Subkingdom",
                Name = "green plants",
                DateUpdated = DateTime.Parse("2012-03-29 00:00:00"),
                Language = "English",
                Subkingdom = "Viridaeplantae",
                Taxon = "Viridaeplantae"
            },
            new Vernacular
            {
                Tsn = "846494",
                Rank = "Infrakingdom",
                Name = "land plants",
                DateUpdated = DateTime.Parse("2012-03-29 00:00:00"),
                Language = "English",
                Infrakingdom = "Streptophyta",
                Taxon = "Streptophyta"
            },
            new Vernacular
            {
                Tsn = "14189",
                Rank = "Phylum",
                Name = "hépatiques",
                DateUpdated = DateTime.Parse("2003-05-21 00:00:00"),
                Language = "French",
                Division = "Bryophyta",
                Taxon = "Bryophyta"
            },
            new Vernacular
            {
                Tsn = "846503",
                Rank = "Subphylum",
                Name = "pteridófita",
                DateUpdated = DateTime.Parse("2012-03-29 00:00:00"),
                Language = "Portuguese",
                Subdivision = "Pteridophytina",
                Taxon = "Pteridophytina"
            },
            new Vernacular
            {
                Tsn = "954908",
                Rank = "Class",
                Name = "conjugating green algae",
                DateUpdated = DateTime.Parse("2014-12-22 00:00:00"),
                Language = "English",
                Class = "Conjugatophyceae",
                Taxon = "Conjugatophyceae"
            },
            new Vernacular
            {
                Tsn = "14198",
                Rank = "Subclass",
                Name = "leafy hepatics",
                DateUpdated = DateTime.Parse("2012-03-11 00:00:00"),
                Language = "English",
                Subclass = "Jungermanniidae",
                Taxon = "Jungermanniidae"
            },
            new Vernacular
            {
                Tsn = "18057",
                Rank = "Order",
                Name = "welwitschia",
                DateUpdated = DateTime.Parse("2012-03-29 00:00:00"),
                Language = "English",
                Order = "Welwitschiales",
                Taxon = "Welwitschiales"
            },
            new Vernacular
            {
                Tsn = "846542",
                Rank = "Superorder",
                Name = "monocotylédones",
                DateUpdated = DateTime.Parse("2012-03-29 00:00:00"),
                Language = "French",
                Superorder = "Lilianae",
                Taxon = "Lilianae"
            },
            new Vernacular
            {
                Tsn = "17169",
                Rank = "Family",
                Name = "adder's-tongue",
                DateUpdated = DateTime.Parse("2012-11-29 00:00:00"),
                Language = "English",
                Family = "Ophioglossaceae",
                Taxon = "Ophioglossaceae"
            },
            new Vernacular
            {
                Tsn = "21049",
                Rank = "Genus",
                Name = "Mary's grass",
                DateUpdated = DateTime.Parse("2011-08-30 00:00:00"),
                Language = "unspecified",
                Genus = "Dedeckera",
                Taxon = "Dedeckera"
            },
            new Vernacular
            {
                Tsn = "14387",
                Rank = "Species",
                Name = "Müller's calypogeja",
                DateUpdated = DateTime.Parse("2008-01-15 00:00:00"),
                Language = "English",
                Species = "Calypogeja muelleriana",
                Taxon = "Calypogeja muelleriana"
            },
            new Vernacular
            {
                Tsn = "15194",
                Rank = "Subspecies",
                Name = "Alaskan ascidiota",
                DateUpdated = DateTime.Parse("2019-09-28 00:00:00"),
                Language = "English",
                Subspecies = "Ascidiota blepharophylla ssp. alaskana",
                Taxon = "Ascidiota blepharophylla ssp. alaskana"
            },
            new Vernacular
            {
                Tsn = "14731",
                Rank = "Variety",
                Name = "fourlobe barbilophozia",
                DateUpdated = DateTime.Parse("2019-09-28 00:00:00"),
                Language = "English",
                Variety = "Barbilophozia quadriloba var. collenchymatica",
                Taxon = "Barbilophozia quadriloba var. collenchymatica"
            },
            new Vernacular
            {
                Tsn = "566322",
                Rank = "Variety",
                Name = "leafless beaked ladies'-tresses",
                DateUpdated = DateTime.Parse("2003-05-09 00:00:00"),
                Language = "unspecified",
                Species = "Stenorrhynchos lanceolatum",
                Variety = "Stenorrhynchos lanceolatum var. paludicola",
                Taxon = "Stenorrhynchos lanceolatum var. paludicola"
            },
            new Vernacular
            {
                Tsn = "837747",
                Rank = "Form",
                Name = "roundleaf serviceberry",
                DateUpdated = DateTime.Parse("2011-12-08 00:00:00"),
                Language = "English",
                Form = "Amelanchier sanguinea f. grandiflora",
                Taxon = "Amelanchier sanguinea f. grandiflora"
            }
        };
    }
}
