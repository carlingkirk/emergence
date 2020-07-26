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
using Microsoft.AspNetCore.Http;

namespace Emergence.Service
{
    public class PhotoService : IPhotoService
    {
        private readonly IBlobService _blobService;
        private readonly IRepository<Photo> _photoRepository;

        public PhotoService(IBlobService blobService, IRepository<Photo> photoRepository)
        {
            _blobService = blobService;
            _photoRepository = photoRepository;
        }

        public async Task<IEnumerable<Data.Shared.Models.Photo>> UploadPhotosAsync(IEnumerable<IFormFile> photos, Data.Shared.Models.PhotoType type, string userId)
        {
            var photoResult = new List<Data.Shared.Models.Photo>();

            foreach (var photo in photos)
            {
                var path = type.ToString() + "/" + userId;
                var name = new Guid().ToString();
                var fileInfo = new FileInfo(photo.FileName);
                name += fileInfo.Extension;

                var result = await _blobService.UploadPhotoAsync(photo, path, name);
                var location = GetLocationFromMetadata(result.Metadata);
                var (length, width) = GetDimensionsFromMetadata(result.Metadata);

                DateTime? dateTaken = null;
                if (result.Metadata.TryGetValue(Constants.DateTaken, out var dateTakenEntry))
                {
                    if (!string.IsNullOrEmpty(dateTakenEntry))
                    {
                        if (DateTime.TryParse(dateTakenEntry, out var dateTakenValue))
                        {
                            dateTaken = dateTakenValue.ToUniversalTime();
                        }
                    }
                }

                photoResult.Add(new Data.Shared.Models.Photo
                {
                    Filename = path + "/" + name,
                    Type = type,
                    UserId = userId,
                    ContentType = result.ContentType,
                    Location = location,
                    Length = length,
                    Width = width,
                    DateTaken = dateTaken,
                    DateCreated = DateTime.UtcNow
                });
            }

            return photoResult;
        }

        public async Task<Data.Shared.Models.Photo> AddOrUpdatePhotoAsync(Data.Shared.Models.Photo photo)
        {
            photo.DateModified = DateTime.UtcNow;
            var photoResult = await _photoRepository.AddOrUpdateAsync(l => l.Id == photo.PhotoId, photo.AsStore());
            return photoResult.AsModel();
        }

        public async Task<bool> AddOrUpdatePhotosAsync(IEnumerable<Data.Shared.Models.Photo> photos)
        {
            await _photoRepository.AddSomeAsync(photos.Select(p => p.AsStore()));
            return true;
        }

        public async Task<IEnumerable<Data.Shared.Models.Photo>> GetPhotosAsync(IEnumerable<int> ids)
        {
            var photoResult = _photoRepository.GetSomeAsync(p => ids.Any(i => i == p.Id));
            var photos = new List<Data.Shared.Models.Photo>();
            await foreach (var photo in photoResult)
            {
                photos.Add(photo.AsModel());
            }
            return photos;
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

        private (int? Length, int? Width) GetDimensionsFromMetadata(IDictionary<string, string> metadata)
        {
            int? length = null;
            int? width = null;

            if (metadata.Count > 0)
            {
                if (metadata.TryGetValue(Constants.Length, out var lengthEntry))
                {
                    if (!string.IsNullOrEmpty(lengthEntry))
                    {
                        if (int.TryParse(lengthEntry, out var lengthValue))
                        {
                            length = lengthValue;
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

            return (length, width);
        }
    }
}
