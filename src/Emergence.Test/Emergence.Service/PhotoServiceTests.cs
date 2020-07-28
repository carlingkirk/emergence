using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Stores;
using Emergence.Service;
using Emergence.Service.Extensions;
using Emergence.Service.Interfaces;
using Emergence.Test.Mocks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using Models = Emergence.Data.Shared.Models;

namespace Emergence.Test.Emergence.API.Services
{
    public class PhotoServiceTests
    {
        private readonly Mock<IRepository<Photo>> _mockPhotoRepository;
        private readonly Mock<IBlobService> _mockBlobService;

        public PhotoServiceTests()
        {
            _mockPhotoRepository = RepositoryMocks.GetStandardMockPhotoRepository();
            _mockBlobService = new Mock<IBlobService>();
        }

        [Fact]
        public async Task TestUploadPhotoAsync()
        {
            var metadata = new Dictionary<string, string>
            {
                { Constants.Latitude, "38.885986" },
                { Constants.Longitude, "-77.036880" },
                { Constants.Altitude, "145" },
                { Constants.Length, "3024" },
                { Constants.Width, "4032" },
                { Constants.DateTaken, "2020-07-22 13:45:26" },
            };
            var mockProperties = new Mock<IBlobResult>();
            mockProperties.Setup(p => p.Metadata).Returns(metadata);
            mockProperties.Setup(p => p.ContentType).Returns("");

            _mockBlobService.Setup(b => b.UploadPhotoAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(mockProperties.Object);

            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");

            var photoService = new PhotoService(_mockBlobService.Object, _mockPhotoRepository.Object);

            var result = await photoService.UploadPhotosAsync(new List<FormFile> { file }, Models.PhotoType.Activity, "me");

            result.FirstOrDefault().Filename.Should().StartWith("activity");
            result.FirstOrDefault().Location.Latitude.Should().Be(38.885986);
            result.FirstOrDefault().Location.Longitude.Should().Be(-77.036880);
            result.FirstOrDefault().Location.Altitude.Should().Be(145);
            result.FirstOrDefault().Length.Should().Be(3024);
            result.FirstOrDefault().Width.Should().Be(4032);
            result.FirstOrDefault().DateTaken.Should().Be(new DateTime(2020, 7, 22, 17, 45, 26, DateTimeKind.Utc));
        }

        [Fact]
        public async Task TestGetPhotosAsync()
        {
            var photoService = new PhotoService(_mockBlobService.Object, _mockPhotoRepository.Object);

            var photos = await photoService.GetPhotosAsync(new int[] { 1, 2, 3 });

            photos.FirstOrDefault().Type.Should().Be(Models.PhotoType.Activity);
            photos.FirstOrDefault().Location.LocationId.Should().Be(1);
        }

        [Fact(Skip = "Integration test")]
        public void TestExifReader()
        {
            using (var stream = new FileStream("D:/Pictures/IMG_20200724_170738.jpg", FileMode.Open, FileAccess.Read))
            {
                var file = new FormFile(stream, 0, 0, "IMG_20200724_170738", "IMG_20200724_170738.jpg");
                var exifReader = new ExifLib.ExifReader(stream);
                var latitude = exifReader.GetLatitude();
                var longitude = exifReader.GetLongitude();
                var altitude = exifReader.GetAltitude();
                var dateTaken = exifReader.GetDateTaken();
                var length = exifReader.GetLength();
                var width = exifReader.GetWidth();

                latitude.Should().Be(34.022947222222221);
                longitude.Should().Be(84.336325);
                altitude.Should().Be(267);
                dateTaken.Should().Be(new DateTime(2020, 7, 24, 17, 07, 38, DateTimeKind.Local));
                length.Should().Be(4032);
                width.Should().Be(3024);
            }
        }
    }
}
