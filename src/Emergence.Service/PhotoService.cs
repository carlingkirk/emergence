using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Interfaces;
using GeoTimeZone;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using TimeZoneConverter;

namespace Emergence.Service
{
    public class PhotoService : IPhotoService
    {
        private readonly IBlobService _blobService;
        private readonly IRepository<Photo> _photoRepository;
        private readonly string _blobStorageRoot;

        public PhotoService(IBlobService blobService, IRepository<Photo> photoRepository, IConfigurationService configurationService)
        {
            _blobService = blobService;
            _photoRepository = photoRepository;
            _blobStorageRoot = configurationService.Settings.BlobStorageRoot + "photos/";
        }

        public async Task<IEnumerable<Data.Shared.Models.Photo>> UploadOriginalsAsync(IEnumerable<IFormFile> photos, PhotoType type, string userId, bool storeLocation = true)
        {
            var photosResult = new List<Data.Shared.Models.Photo>();

            foreach (var photo in photos)
            {
                var photoResult = await UploadPhoto(photo, type, userId, storeLocation);
                photosResult.Add(photoResult);
            }

            return photosResult;
        }

        public async Task<Data.Shared.Models.Photo> UploadOriginalAsync(IFormFile photo, PhotoType type, string userId, bool storeLocation = true)
        {
            var photoResult = await UploadPhoto(photo, type, userId, storeLocation);
            return photoResult;
        }

        public async Task<Data.Shared.Models.Photo> AddOrUpdatePhotoAsync(Data.Shared.Models.Photo photo)
        {
            photo.DateModified = DateTime.UtcNow;
            var photoResult = await _photoRepository.AddOrUpdateAsync(l => l.Id == photo.PhotoId, photo.AsStore());

            return photoResult.AsModel(_blobStorageRoot);
        }

        public async Task<IEnumerable<Data.Shared.Models.Photo>> AddOrUpdatePhotosAsync(IEnumerable<Data.Shared.Models.Photo> photos)
        {
            var updates = photos.Where(p => p.PhotoId != 0);
            var updateResult = await _photoRepository.UpdateSomeAsync(updates.Select(p => p.AsStore()));

            var adds = photos.Where(p => p.PhotoId == 0);
            var addResult = await _photoRepository.AddSomeAsync(adds.Select(p => p.AsStore()));

            return updateResult.Union(addResult).Select(p => p.AsModel(_blobStorageRoot));
        }

        public async Task<IEnumerable<Data.Shared.Models.Photo>> GetPhotosAsync(IEnumerable<int> ids)
        {
            var photoResult = _photoRepository.GetSomeAsync(p => ids.Any(i => i == p.Id));
            var photos = new List<Data.Shared.Models.Photo>();
            await foreach (var photo in photoResult)
            {
                photos.Add(photo.AsModel(_blobStorageRoot));
            }
            return photos;
        }

        public async Task<Data.Shared.Models.Photo> GetPhotoAsync(int id)
        {
            var photoResult = await _photoRepository.GetAsync(p => p.Id == id);
            var photo = photoResult.AsModel(_blobStorageRoot);
            return photo;
        }

        public async Task<IEnumerable<Data.Shared.Models.Photo>> GetPhotosAsync(PhotoType type, int typeId)
        {
            var photoResult = _photoRepository.GetSomeAsync(p => p.Type == type.ToString() && p.TypeId == typeId);
            var photos = new List<Data.Shared.Models.Photo>();
            await foreach (var photo in photoResult)
            {
                photos.Add(photo.AsModel(_blobStorageRoot));
            }
            return photos;
        }

        public async Task<bool> RemovePhotoAsync(int id, string userId)
        {
            var photo = await GetPhotoAsync(id);
            await _blobService.RemovePhotoAsync(photo.BlobPath);
            var result = await _photoRepository.RemoveAsync(photo.AsStore());
            return result;
        }

        public async Task<Image> ProcessPhotoAsync(Stream stream, Image image, Data.Shared.Models.ImageSize imageSize)
        {
            image = OrientPhoto(image);
            image = ResizePhoto(image, (int)imageSize);
            image = await SaveAsPngAsync(stream, image);

            return image;
        }

        public async Task RemovePhotosAsync(IEnumerable<Data.Shared.Models.Photo> photos)
        {
            foreach (var photo in photos)
            {
                await _blobService.RemovePhotoAsync(photo.BlobPath);
            }
            await _photoRepository.RemoveManyAsync(photos.Select(p => p.AsStore()));
        }

        private async Task<Data.Shared.Models.Photo> UploadPhoto(IFormFile photo, PhotoType type, string userId, bool storeLocation)
        {
            var blobpath = Guid.NewGuid().ToString();
            var fileInfo = new FileInfo(photo.FileName);
            var name = "original" + fileInfo.Extension;

            var result = await _blobService.UploadPhotoAsync(photo, userId, Path.Combine(blobpath, name));
            if (result != null)
            {
                var location = GetLocationFromMetadata(result.Metadata);
                var (height, width) = GetDimensionsFromMetadata(result.Metadata);
                var timezone = TimeZoneInfo.Local;
                if (location != null && location.Latitude.HasValue && location.Longitude.HasValue)
                {
                    var zone = TimeZoneLookup.GetTimeZone(location.Latitude.Value, location.Longitude.Value);
                    timezone = TZConvert.GetTimeZoneInfo(zone.Result);
                }

                DateTime? dateTaken = null;
                if (result.Metadata.TryGetValue(Constants.DateTaken, out var dateTakenEntry))
                {
                    if (!string.IsNullOrEmpty(dateTakenEntry))
                    {
                        if (DateTime.TryParse(dateTakenEntry, out var dateTakenValue))
                        {
                            dateTaken = TimeZoneInfo.ConvertTimeToUtc(dateTakenValue, timezone);
                        }
                    }
                }

                return new Data.Shared.Models.Photo
                {
                    Filename = name,
                    BlobPath = blobpath,
                    BlobPathRoot = _blobStorageRoot,
                    Type = type,
                    UserId = userId,
                    ContentType = result.ContentType,
                    Location = storeLocation ? location : null,
                    Height = height,
                    Width = width,
                    DateTaken = dateTaken,
                    DateCreated = DateTime.UtcNow,
                    CreatedBy = userId
                };
            }

            return null;
        }

        private Image ResizePhoto(Image image, int maxDimension)
        {
            if (image.Width > maxDimension || image.Height > maxDimension)
            {
                var aspectRatio = image.Width / (double)image.Height;
                var newWidth = (int)Math.Floor(aspectRatio > 1 ? maxDimension : maxDimension / aspectRatio);
                var newHeight = (int)Math.Floor(aspectRatio < 1 ? maxDimension : maxDimension / aspectRatio);
                return image.Clone(i => i.Resize(newWidth, newHeight));
            }
            return image;
        }

        private async Task<Image> SaveAsPngAsync(Stream stream, Image image)
        {
            var pngEncoder = new PngEncoder
            {
                CompressionLevel = PngCompressionLevel.BestCompression,
                BitDepth = PngBitDepth.Bit8,
                IgnoreMetadata = true,
                FilterMethod = PngFilterMethod.Adaptive
            };

            await image.SaveAsPngAsync(stream, pngEncoder);

            return image;
        }

        private Image OrientPhoto(Image image)
        {
            image.Mutate(i => i.AutoOrient());
            return image;
        }

        private Data.Shared.Models.Location GetLocationFromMetadata(IDictionary<string, string> metadata)
        {
            var location = new Data.Shared.Models.Location();
            var hasValues = false;

            if (metadata.Count > 0)
            {
                if (metadata.TryGetValue(Constants.Latitude, out var latitude))
                {
                    if (!string.IsNullOrEmpty(latitude))
                    {
                        if (double.TryParse(latitude, out var latValue))
                        {
                            location.Latitude = latValue;
                            hasValues = true;
                        }
                    }
                }
                if (metadata.TryGetValue(Constants.Longitude, out var longitude))
                {
                    if (!string.IsNullOrEmpty(longitude))
                    {
                        if (double.TryParse(longitude, out var longValue))
                        {
                            location.Longitude = longValue;
                            hasValues = true;
                        }
                    }
                }

                if (metadata.TryGetValue(Constants.Altitude, out var altitude))
                {
                    if (!string.IsNullOrEmpty(altitude))
                    {
                        if (double.TryParse(altitude, out var altValue))
                        {
                            location.Altitude = altValue;
                            hasValues = true;
                        }
                    }
                }
            }

            return hasValues ? location : null;
        }

        private (int? Height, int? Width) GetDimensionsFromMetadata(IDictionary<string, string> metadata)
        {
            int? height = null;
            int? width = null;

            if (metadata.Count > 0)
            {
                if (metadata.TryGetValue(Constants.Height, out var heightEntry))
                {
                    if (!string.IsNullOrEmpty(heightEntry))
                    {
                        if (int.TryParse(heightEntry, out var heightValue))
                        {
                            height = heightValue;
                        }
                    }
                }

                if (metadata.TryGetValue(Constants.Width, out var widthEntry))
                {
                    if (!string.IsNullOrEmpty(widthEntry))
                    {
                        if (int.TryParse(widthEntry, out var widthValue))
                        {
                            width = widthValue;
                        }
                    }
                }
            }

            return (height, width);
        }
    }
}
