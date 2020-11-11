using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Emergence.Data;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Stores;
using MockQueryable.Moq;
using Moq;

namespace Emergence.Test.Mocks
{
    public static class RepositoryMocks
    {
        public static Mock<IRepository<Specimen>> GetStandardMockSpecimenRepository(IEnumerable<Specimen> specimens = null)
        {
            if (specimens == null)
            {
                specimens = Data.Fakes.Stores.FakeSpecimens.Get();
            }

            var mockSpecimens = specimens.AsQueryable().BuildMockDbSet().Object;
            var mockSpecimenRepo = new Mock<IRepository<Specimen>>();

            mockSpecimenRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Specimen, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(mockSpecimens.FirstOrDefault());

            mockSpecimenRepo.Setup(p => p.GetWithIncludesAsync(It.IsAny<Expression<Func<Specimen, bool>>>(), It.IsAny<bool>(),
                It.IsAny<Func<IIncludable<Specimen>, IIncludable>[]>()))
                .ReturnsAsync(mockSpecimens.FirstOrDefault());

            mockSpecimenRepo.Setup(p => p.Where(It.IsAny<Expression<Func<Specimen, bool>>>(), It.IsAny<bool>()))
                .Returns(mockSpecimens);

            mockSpecimenRepo.Setup(p => p.WhereWithIncludes(It.IsAny<Expression<Func<Specimen, bool>>>(), It.IsAny<bool>(),
                It.IsAny<Func<IIncludable<Specimen>, IIncludable>[]>()))
                .Returns(mockSpecimens);

            mockSpecimenRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<Specimen, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(mockSpecimens);

            return mockSpecimenRepo;
        }

        public static Mock<IRepository<User>> GetStandardMockUserRepository(IEnumerable<User> users = null)
        {
            if (users == null)
            {
                users = Data.Fakes.Stores.FakeUsers.Get();
            }

            var mockUsers = users.AsQueryable().BuildMockDbSet().Object;
            var mockUsersRepo = new Mock<IRepository<User>>();

            mockUsersRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(mockUsers.FirstOrDefault());

            mockUsersRepo.Setup(p => p.GetWithIncludesAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(),
                It.IsAny<Func<IIncludable<User>, IIncludable>[]>()))
                .ReturnsAsync(mockUsers.FirstOrDefault());

            mockUsersRepo.Setup(p => p.Where(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>()))
                .Returns(mockUsers);

            mockUsersRepo.Setup(p => p.WhereWithIncludes(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(),
                It.IsAny<Func<IIncludable<User>, IIncludable>[]>()))
                .Returns(mockUsers);

            mockUsersRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(mockUsers);

            return mockUsersRepo;
        }

        public static Mock<IRepository<Origin>> GetStandardMockOriginRepository(IEnumerable<Origin> origins = null)
        {
            var random = new Random();
            if (origins == null)
            {
                origins = Data.Fakes.Stores.FakeOrigins.Get();
            }

            var mockOriginRepo = new Mock<IRepository<Origin>>();
            var mockOrigins = origins.AsQueryable().BuildMockDbSet().Object;

            mockOriginRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Origin, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(mockOrigins.FirstOrDefault());

            mockOriginRepo.Setup(o => o.GetWithIncludesAsync(It.IsAny<Expression<Func<Origin, bool>>>(), It.IsAny<bool>(),
                It.IsAny<Func<IIncludable<Origin>, IIncludable>[]>()))
                .ReturnsAsync(mockOrigins.FirstOrDefault());

            mockOriginRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<Origin, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(mockOrigins.ToAsyncEnumerable());

            mockOriginRepo.Setup(p => p.AddSomeAsync(It.IsAny<IEnumerable<Origin>>()))
                .ReturnsAsync((IEnumerable<Origin> origins) =>
                {
                    origins = origins.ToList();
                    foreach (var origin in origins)
                    {
                        origin.Id = random.Next(1, int.MaxValue);
                    }
                    return origins;
                });

            mockOriginRepo.Setup(p => p.AddOrUpdateAsync(It.IsAny<Expression<Func<Origin, bool>>>(), It.IsAny<Origin>()))
                .ReturnsAsync((Expression<Func<Origin, bool>> expr, Origin origin) =>
                {
                    origin.Id = random.Next(1, int.MaxValue);

                    return origin;
                });

            mockOriginRepo.Setup(p => p.WhereWithIncludes(It.IsAny<Expression<Func<Origin, bool>>>(), It.IsAny<bool>(),
                It.IsAny<Func<IIncludable<Origin>, IIncludable>[]>()))
                .Returns(mockOrigins);

            mockOriginRepo.Setup(p => p.Where(It.IsAny<Expression<Func<Origin, bool>>>(), It.IsAny<bool>()))
                .Returns(mockOrigins);

            return mockOriginRepo;
        }

        public static Mock<IRepository<Lifeform>> GetStandardMockLifeformRepository(IEnumerable<Lifeform> lifeforms = null)
        {
            var random = new Random();
            if (lifeforms == null)
            {
                lifeforms = Data.Fakes.Stores.FakeLifeforms.Get();
            }

            var mockLifeforms = lifeforms.AsQueryable().BuildMockDbSet().Object;
            var mockLifeformRepo = new Mock<IRepository<Lifeform>>();

            mockLifeformRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<Lifeform, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(mockLifeforms.ToAsyncEnumerable());

            mockLifeformRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Lifeform, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(mockLifeforms.FirstOrDefault());

            mockLifeformRepo.Setup(p => p.AddOrUpdateAsync(It.IsAny<Expression<Func<Lifeform, bool>>>(), It.IsAny<Lifeform>()))
                .ReturnsAsync((Expression<Func<Lifeform, bool>> expr, Lifeform lifeform) =>
                {
                    lifeform.Id = random.Next(1, int.MaxValue);

                    return lifeform;
                });

            return mockLifeformRepo;
        }

        public static Mock<IRepository<Location>> GetStandardMockLocationRepository(IEnumerable<Location> locations = null)
        {
            var random = new Random();
            if (locations == null)
            {
                locations = Data.Fakes.Stores.FakeLocations.Get();
            }

            var mockLocations = locations.AsQueryable().BuildMockDbSet().Object;
            var mockLocationRepo = new Mock<IRepository<Location>>();

            mockLocationRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<Location, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(mockLocations.ToAsyncEnumerable());

            mockLocationRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Location, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(mockLocations.FirstOrDefault());

            mockLocationRepo.Setup(p => p.AddSomeAsync(It.IsAny<IEnumerable<Location>>()))
                .ReturnsAsync((IEnumerable<Location> locations) =>
                {
                    locations = locations.ToList();
                    foreach (var location in locations)
                    {
                        location.Id = random.Next(1, int.MaxValue);
                    }
                    return locations;
                });

            return mockLocationRepo;
        }

        public static Mock<IRepository<Synonym>> GetStandardMockSynonymRepository(IEnumerable<Synonym> synonyms = null)
        {
            var random = new Random();
            if (synonyms == null)
            {
                synonyms = Data.Fakes.Stores.FakeSynonyms.Get();
            }

            var mockSynonymRepo = new Mock<IRepository<Synonym>>();
            var mockSynonyms = synonyms.AsQueryable().BuildMockDbSet().Object;

            mockSynonymRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Synonym, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(mockSynonyms.FirstOrDefault());

            mockSynonymRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<Synonym, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(mockSynonyms.ToAsyncEnumerable());

            mockSynonymRepo.Setup(p => p.AddSomeAsync(It.IsAny<IEnumerable<Synonym>>()))
                .ReturnsAsync((IEnumerable<Synonym> synonyms) =>
                {
                    synonyms = synonyms.ToList();
                    foreach (var synonym in synonyms)
                    {
                        synonym.Id = random.Next(1, int.MaxValue);
                    }
                    return synonyms;
                });

            mockSynonymRepo.Setup(p => p.AddOrUpdateAsync(It.IsAny<Expression<Func<Synonym, bool>>>(), It.IsAny<Synonym>()))
                .ReturnsAsync((Expression<Func<Synonym, bool>> expr, Synonym synonym) =>
                {
                    synonym.Id = random.Next(1, int.MaxValue);

                    return synonym;
                });

            return mockSynonymRepo;
        }

        public static Mock<IRepository<InventoryItem>> GetStandardMockInventoryItemRepository(IEnumerable<InventoryItem> inventoryItems = null)
        {
            if (inventoryItems == null)
            {
                inventoryItems = Data.Fakes.Stores.FakeInventories.GetItems();
            }

            var mockInventoryItems = inventoryItems.AsQueryable().BuildMockDbSet().Object;
            var mockInventoryItemRepo = new Mock<IRepository<InventoryItem>>();

            mockInventoryItemRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<InventoryItem, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(mockInventoryItems.ToAsyncEnumerable());

            return mockInventoryItemRepo;
        }

        public static Mock<IRepository<Inventory>> GetStandardMockInventoryRepository(IEnumerable<Inventory> inventories = null)
        {
            if (inventories == null)
            {
                inventories = Data.Fakes.Stores.FakeInventories.Get();
            }

            var mockInventories = inventories.AsQueryable().BuildMockDbSet().Object;
            var mockInventoryRepo = new Mock<IRepository<Inventory>>();

            mockInventoryRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Inventory, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(mockInventories.FirstOrDefault());

            return mockInventoryRepo;
        }

        public static Mock<IRepository<PlantInfo>> GetStandardMockPlantInfoRepository(IEnumerable<PlantInfo> plantInfos = null)
        {
            var random = new Random();
            if (plantInfos == null)
            {
                plantInfos = Data.Fakes.Stores.FakePlantInfos.Get();
            }

            var mockPlantInfos = plantInfos.AsQueryable().BuildMockDbSet().Object;
            var mockPlantInfoRepo = new Mock<IRepository<PlantInfo>>();

            mockPlantInfoRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<PlantInfo, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(mockPlantInfos.FirstOrDefault());

            mockPlantInfoRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<PlantInfo, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(mockPlantInfos.ToAsyncEnumerable());

            mockPlantInfoRepo.Setup(p => p.AddSomeAsync(It.IsAny<IEnumerable<PlantInfo>>()))
                .ReturnsAsync((IEnumerable<PlantInfo> plantInfos) =>
                {
                    plantInfos = plantInfos.ToList();
                    foreach (var plantInfo in plantInfos)
                    {
                        plantInfo.Id = random.Next(1, int.MaxValue);
                    }
                    return plantInfos;
                });

            mockPlantInfoRepo.Setup(p => p.AddOrUpdateAsync(It.IsAny<Expression<Func<PlantInfo, bool>>>(), It.IsAny<PlantInfo>()))
                .ReturnsAsync((Expression<Func<PlantInfo, bool>> expr, PlantInfo plantInfo) =>
                {
                    plantInfo.Id = random.Next(1, int.MaxValue);
                    return plantInfo;
                });

            mockPlantInfoRepo.Setup(p => p.GetWithIncludesAsync(It.IsAny<Expression<Func<PlantInfo, bool>>>(), It.IsAny<bool>(),
                It.IsAny<Func<IIncludable<PlantInfo>, IIncludable>[]>()))
                .ReturnsAsync(mockPlantInfos.FirstOrDefault());

            mockPlantInfoRepo.Setup(p => p.WhereWithIncludes(It.IsAny<Expression<Func<PlantInfo, bool>>>(), It.IsAny<bool>(),
                It.IsAny<Func<IIncludable<PlantInfo>, IIncludable>[]>()))
                .Returns(mockPlantInfos);

            mockPlantInfoRepo.Setup(p => p.Where(It.IsAny<Expression<Func<PlantInfo, bool>>>(), It.IsAny<bool>()))
                .Returns(mockPlantInfos);

            return mockPlantInfoRepo;
        }

        public static Mock<IRepository<Activity>> GetStandardMockActivityRepository(IEnumerable<Activity> activities = null)
        {
            if (activities == null)
            {
                activities = Data.Fakes.Stores.FakeActivities.Get();
            }

            var mockActivities = activities.AsQueryable().BuildMockDbSet().Object;
            var mockActivityRepo = new Mock<IRepository<Activity>>();

            mockActivityRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(mockActivities.FirstOrDefault());

            mockActivityRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(mockActivities.ToAsyncEnumerable());

            mockActivityRepo.Setup(p => p.WhereWithIncludes(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<bool>(),
                It.IsAny<Func<IIncludable<Activity>, IIncludable>[]>()))
                .Returns(mockActivities);

            return mockActivityRepo;
        }

        public static Mock<IRepository<Photo>> GetStandardMockPhotoRepository(IEnumerable<Photo> photos = null)
        {
            var random = new Random();
            if (photos == null)
            {
                photos = Data.Fakes.Stores.FakePhotos.Get();
            }

            var mockPhotos = photos.AsQueryable().BuildMockDbSet().Object;
            var mockPhotoRepo = new Mock<IRepository<Photo>>();

            mockPhotoRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Photo, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(mockPhotos.FirstOrDefault());

            mockPhotoRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<Photo, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(mockPhotos.ToAsyncEnumerable());

            mockPhotoRepo.Setup(p => p.AddOrUpdateAsync(It.IsAny<Expression<Func<Photo, bool>>>(), It.IsAny<Photo>()))
                .ReturnsAsync((Expression<Func<Photo, bool>> expr, Photo photo) =>
                {
                    photo.Id = random.Next(1, int.MaxValue);
                    return photo;
                });

            return mockPhotoRepo;
        }

        public static Mock<IRepository<PlantLocation>> GetStandardMockPlantLocationRepository(IEnumerable<PlantLocation> plantLocations = null)
        {
            var random = new Random();
            if (plantLocations == null)
            {
                plantLocations = Data.Fakes.Stores.FakePlantLocations.Get();
            }

            var mockPlantLocations = plantLocations.AsQueryable().BuildMockDbSet().Object;
            var mockPlantLocationRepo = new Mock<IRepository<PlantLocation>>();

            mockPlantLocationRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<PlantLocation, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(mockPlantLocations.ToAsyncEnumerable());

            mockPlantLocationRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<PlantLocation, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(mockPlantLocations.FirstOrDefault());

            mockPlantLocationRepo.Setup(p => p.AddSomeAsync(It.IsAny<IEnumerable<PlantLocation>>()))
                .ReturnsAsync((IEnumerable<PlantLocation> plantLocations) =>
                {
                    plantLocations = plantLocations.ToList();
                    foreach (var plantLocation in plantLocations)
                    {
                        plantLocation.Id = random.Next(1, int.MaxValue);
                    }
                    return plantLocations;
                });

            return mockPlantLocationRepo;
        }

        public static Mock<IRepository<Taxon>> GetStandardMockTaxonRepository(IEnumerable<Taxon> taxons = null)
        {
            var random = new Random();
            if (taxons == null)
            {
                taxons = Data.Fakes.Stores.FakeTaxons.Get();
            }

            var mockTaxons = taxons.AsQueryable().BuildMockDbSet().Object;
            var mockTaxonRepo = new Mock<IRepository<Taxon>>();

            mockTaxonRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<Taxon, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(mockTaxons.ToAsyncEnumerable());

            mockTaxonRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Taxon, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(mockTaxons.FirstOrDefault());

            mockTaxonRepo.Setup(p => p.AddOrUpdateAsync(It.IsAny<Expression<Func<Taxon, bool>>>(), It.IsAny<Taxon>()))
                .ReturnsAsync((Expression<Func<Taxon, bool>> expr, Taxon taxon) =>
                {
                    taxon.Id = random.Next(1, int.MaxValue);
                    return taxon;
                });

            return mockTaxonRepo;
        }

        public static Mock<IRepository<UserContact>> GetStandardMockUserContactRepository(IEnumerable<UserContact> userContacts = null)
        {
            var random = new Random();
            if (userContacts == null)
            {
                userContacts = Data.Fakes.Stores.FakeUserContacts.GetContacts();
            }

            var mockUserContacts = userContacts.AsQueryable().BuildMockDbSet().Object;
            var mockUserContactRepo = new Mock<IRepository<UserContact>>();

            mockUserContactRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<UserContact, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(mockUserContacts.ToAsyncEnumerable());

            mockUserContactRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<UserContact, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(mockUserContacts.FirstOrDefault());

            mockUserContactRepo.Setup(p => p.AddSomeAsync(It.IsAny<IEnumerable<UserContact>>()))
                .ReturnsAsync((IEnumerable<UserContact> userContacts) =>
                {
                    userContacts = userContacts.AsQueryable().ToList();
                    foreach (var userContact in userContacts)
                    {
                        userContact.Id = random.Next(1, int.MaxValue);
                    }
                    return userContacts;
                });

            mockUserContactRepo.Setup(p => p.AddOrUpdateAsync(It.IsAny<Expression<Func<UserContact, bool>>>(), It.IsAny<UserContact>()))
                .ReturnsAsync((Expression<Func<UserContact, bool>> expr, UserContact userContact) =>
                {
                    userContact.Id = random.Next(1, int.MaxValue);

                    return userContact;
                });

            mockUserContactRepo.Setup(p => p.Where(It.IsAny<Expression<Func<UserContact, bool>>>(), It.IsAny<bool>()))
                .Returns(mockUserContacts);

            mockUserContactRepo.Setup(p => p.WhereWithIncludes(It.IsAny<Expression<Func<UserContact, bool>>>(), It.IsAny<bool>(),
                It.IsAny<Func<IIncludable<UserContact>, IIncludable>[]>()))
                .Returns(mockUserContacts);

            mockUserContactRepo.Setup(ucr => ucr.RemoveAsync(It.IsAny<UserContact>())).ReturnsAsync(true);

            return mockUserContactRepo;
        }

        public static Mock<IRepository<UserContactRequest>> GetStandardMockUserContactRequestRepository(IEnumerable<UserContactRequest> userContactRequests = null)
        {
            var random = new Random();
            if (userContactRequests == null)
            {
                userContactRequests = Data.Fakes.Stores.FakeUserContacts.GetContactRequests();
            }

            var mockUserContactRequests = userContactRequests.AsQueryable().BuildMockDbSet().Object;
            var mockUserContactRequestRepo = new Mock<IRepository<UserContactRequest>>();

            mockUserContactRequestRepo.Setup(ucr => ucr.GetSomeAsync(It.IsAny<Expression<Func<UserContactRequest, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(mockUserContactRequests.ToAsyncEnumerable());

            mockUserContactRequestRepo.Setup(ucr => ucr.GetAsync(It.IsAny<Expression<Func<UserContactRequest, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(mockUserContactRequests.FirstOrDefault());

            mockUserContactRequestRepo.Setup(ucr => ucr.AddSomeAsync(It.IsAny<IEnumerable<UserContactRequest>>()))
                .ReturnsAsync((IEnumerable<UserContactRequest> userContactRequests) =>
                {
                    userContactRequests = userContactRequests.AsQueryable().ToList();
                    foreach (var userContactRequest in userContactRequests)
                    {
                        userContactRequest.Id = random.Next(1, int.MaxValue);
                    }
                    return userContactRequests;
                });

            mockUserContactRequestRepo.Setup(p => p.AddOrUpdateAsync(It.IsAny<Expression<Func<UserContactRequest, bool>>>(), It.IsAny<UserContactRequest>()))
                .ReturnsAsync((Expression<Func<UserContactRequest, bool>> expr, UserContactRequest userContactRequest) =>
                {
                    userContactRequest.Id = random.Next(1, int.MaxValue);

                    return userContactRequest;
                });

            mockUserContactRequestRepo.Setup(ucr => ucr.Where(It.IsAny<Expression<Func<UserContactRequest, bool>>>(), It.IsAny<bool>()))
                .Returns(mockUserContactRequests);

            mockUserContactRequestRepo.Setup(ucr => ucr.WhereWithIncludes(It.IsAny<Expression<Func<UserContactRequest, bool>>>(), It.IsAny<bool>(),
                It.IsAny<Func<IIncludable<UserContactRequest>, IIncludable>[]>()))
                .Returns(mockUserContactRequests);

            mockUserContactRequestRepo.Setup(ucr => ucr.RemoveAsync(It.IsAny<UserContactRequest>())).ReturnsAsync(true);

            return mockUserContactRequestRepo;
        }
    }
}
