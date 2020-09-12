using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.External.ITIS;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Emergence.Service;
using Emergence.Service.Extensions;
using Emergence.Test.Mocks;
using Emergence.Transform;
using FluentAssertions;
using Xunit;
using Stores = Emergence.Data.Shared.Stores;

namespace Emergence.Test.Emergence.Transform
{
    public class ITISTests
    {
        [Fact]
        public void TestITISTransformer()
        {
            var transformer = new ITISPlantInfoTransformer();

            var itisData = ITISData();
            var result = new List<PlantInfo>();
            itisData.ForEach(i => result.AddRange(transformer.Transform(new List<TaxonomicUnit> { i })));

            result.Where(p => p.ScientificName == "Glandularia quadrangulata").Count().Should().Be(1);
            result.Where(p => p.Taxon.Subfamily == null).Count().Should().Be(6);
            result.Where(p => p.Taxon.Species == "cremersii").Count().Should().Be(2);
            result.Where(p => p.Taxon.Form == "viridifolia").Count().Should().Be(2);
            result.Where(p => p.Taxon.Subspecies == "purpurea").Count().Should().Be(1);
            result.Where(p => p.Taxon.Variety == "graminea").Count().Should().Be(1);
            result.Where(p => p.Locations != null).SelectMany(p => p.Locations).Count().Should().Be(3);
            result.SelectMany(p => p.Locations).DistinctBy(l => l.Location.Region).Count().Should().Be(1);
            result.Select(p => p.Origin).Count().Should().Be(6);
        }

        [Fact]
        public async Task TestITISProcessor()
        {
            var transformer = new ITISPlantInfoTransformer();
            var itisData = ITISData();
            var plantInfos = new List<PlantInfo>();
            itisData.ForEach(i => plantInfos.AddRange(transformer.Transform(new List<TaxonomicUnit> { i })));

            var originRepository = RepositoryMocks.GetStandardMockOriginRepository(new List<Stores.Origin>());
            var locationRepository = RepositoryMocks.GetStandardMockLocationRepository(new List<Stores.Location>());
            var lifeformRepository = RepositoryMocks.GetStandardMockLifeformRepository(new List<Stores.Lifeform>());
            var plantInfoRepository = RepositoryMocks.GetStandardMockPlantInfoRepository(new List<Stores.PlantInfo>());
            var plantLocationRepository = RepositoryMocks.GetStandardMockPlantLocationRepository(new List<Stores.PlantLocation>());
            var taxonRepository = RepositoryMocks.GetStandardMockTaxonRepository(new List<Stores.Taxon>());

            var locationService = new LocationService(locationRepository.Object);
            var originService = new OriginService(originRepository.Object, locationService);
            var lifeformService = new LifeformService(lifeformRepository.Object);
            var plantInfoService = new PlantInfoService(plantInfoRepository.Object, plantLocationRepository.Object);
            var taxonService = new TaxonService(taxonRepository.Object);

            var processor = new PlantInfoProcessor(lifeformService, originService, plantInfoService, taxonService, locationService);

            await processor.InitializeOrigin(transformer.Origin);
            await processor.InitializeLifeforms();
            await processor.InitializeTaxons();

            var result = await processor.Process(plantInfos);

            result.Where(p => p.ScientificName == "Glandularia quadrangulata").Count().Should().Be(1);
            result.Where(p => p.Taxon.Subfamily == null).Count().Should().Be(5);
            result.Where(p => p.Taxon.Species == "cremersii").Count().Should().Be(1);
            result.Where(p => p.Taxon.Form == "viridifolia").Count().Should().Be(1);
            result.Where(p => p.Taxon.Subspecies == "purpurea").Count().Should().Be(1);
            result.Where(p => p.Taxon.Variety == "graminea").Count().Should().Be(1);
            result.Where(p => p.Locations != null).SelectMany(p => p.Locations).Count().Should().Be(3);
            result.Where(p => p.Locations != null).SelectMany(p => p.Locations).Count(l => l.Status == LocationStatus.Native).Should().Be(3);
            result.Select(p => p.Origin).DistinctBy(o => o.OriginId).Count().Should().Be(5);
        }

        private static List<TaxonomicUnit> ITISData() => new List<TaxonomicUnit>
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
    }
}
