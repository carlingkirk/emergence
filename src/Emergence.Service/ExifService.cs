using System;
using System.Collections.Generic;
using Emergence.Service.Interfaces;
using ExifLib;
using Microsoft.Extensions.Logging;

namespace Emergence.Service
{
    public class ExifService : IExifService
    {
        private readonly ILogger<IExifService> _logger;
        public ExifService(ILogger<IExifService> logger)
        {
            _logger = logger;
        }

        public Dictionary<string, string> GetMetadata(ExifReader exifReader)
        {
            var metadata = new Dictionary<string, string>();
            var latitude = GetLatitude(exifReader);
            var longitude = GetLongitude(exifReader);
            var altitude = GetAltitude(exifReader);
            var height = GetLength(exifReader);
            var width = GetWidth(exifReader);
            var dateTaken = GetDateTaken(exifReader);

            if (latitude.HasValue)
            {
                metadata.Add("Latitude", latitude.Value.ToString());
            }
            if (longitude.HasValue)
            {
                metadata.Add("Longitude", longitude.Value.ToString());
            }
            if (dateTaken.HasValue)
            {
                metadata.Add("DateTaken", dateTaken.Value.ToString());
            }
            if (altitude.HasValue)
            {
                metadata.Add("Altitude", altitude.Value.ToString());
            }
            if (height.HasValue)
            {
                metadata.Add("Height", height.Value.ToString());
            }
            if (width.HasValue)
            {
                metadata.Add("Width", width.Value.ToString());
            }

            return metadata;
        }

        private double? GetLatitude(ExifReader exifReader) => GetCoordinate(ExifTags.GPSLatitude, ExifTags.GPSLatitudeRef, exifReader);

        private double? GetLongitude(ExifReader exifReader) => GetCoordinate(ExifTags.GPSLongitude, ExifTags.GPSLongitudeRef, exifReader);

        private DateTime? GetDateTaken(ExifReader exifReader)
        {
            try
            {
                if (exifReader.GetTagValue<DateTime>(ExifTags.DateTime, out var dateTaken))
                {
                    return dateTaken;
                }
                else if (exifReader.GetTagValue(ExifTags.GPSDateStamp, out dateTaken))
                {
                    return dateTaken;
                }
                else if (exifReader.GetTagValue(ExifTags.DateTimeOriginal, out dateTaken))
                {
                    return dateTaken;
                }
                else if (exifReader.GetTagValue(ExifTags.DateTimeDigitized, out dateTaken))
                {
                    return dateTaken;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error reading tag DateTaken", ex);
            }

            return null;
        }

        private double? GetAltitude(ExifReader exifReader)
        {
            try
            {
                if (exifReader.GetTagValue<double>(ExifTags.GPSAltitude, out var altitude))
                {
                    return altitude;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error reading tag GPSAltitude", ex);
            }

            return null;
        }

        private ulong? GetLength(ExifReader exifReader)
        {
            try
            {
                if (exifReader.GetTagValue<uint>(ExifTags.ImageLength, out var length))
                {
                    return length;
                }
            }
            catch (InvalidCastException ex)
            {
                _logger.LogWarning("Error reading tag ImageLength as uint", ex);
            }

            try
            {
                if (exifReader.GetTagValue<ushort>(ExifTags.ImageLength, out var length))
                {
                    return length;
                }
            }
            catch (InvalidCastException ex)
            {
                _logger.LogError("Error reading tag ImageLength as ushort", ex);
            }

            try
            {
                if (exifReader.GetTagValue<ulong>(ExifTags.ImageLength, out var length))
                {
                    return length;
                }
            }
            catch (InvalidCastException ex)
            {
                _logger.LogError("Error reading tag ImageLength as ulong, giving up", ex);
            }

            return null;
        }

        private ulong? GetWidth(ExifReader exifReader)
        {
            try
            {
                if (exifReader.GetTagValue<uint>(ExifTags.ImageWidth, out var width))
                {
                    return width;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error reading tag ImageWidth as uint", ex);
            }

            try
            {
                if (exifReader.GetTagValue<ushort>(ExifTags.ImageWidth, out var width))
                {
                    return width;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error reading tag ImageWidth as ushort", ex);
            }

            try
            {
                if (exifReader.GetTagValue<ulong>(ExifTags.ImageWidth, out var width))
                {
                    return width;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error reading tag ImageWidth as ulong, giving up", ex);
            }

            return null;
        }

        private double? GetCoordinate(ExifTags type, ExifTags refType, ExifReader exifReader)
        {
            try
            {
                if (exifReader.GetTagValue(type, out double[] coordinates))
                {
                    var location = ToDoubleCoordinates(coordinates);
                    if (exifReader.GetTagValue(refType, out string cardinal))
                    {
                        return cardinal == "N" || cardinal == "E" ? location : location * -1;
                    }
                    else
                    {
                        return location;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error reading coordinates for {type}", ex);
            }
            return null;
        }

        private double ToDoubleCoordinates(double[] coordinates) => coordinates[0] + (coordinates[1] / 60f) + (coordinates[2] / 3600f);
    }
}
