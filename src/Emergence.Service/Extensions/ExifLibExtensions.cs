using System;
using ExifLib;

namespace Emergence.Service.Extensions
{
    public static class ExifLibExtensions
    {
        public static double? GetLatitude(this ExifReader reader) => reader.GetCoordinate(ExifTags.GPSLatitude);

        public static double? GetLongitude(this ExifReader reader) => reader.GetCoordinate(ExifTags.GPSLongitude);

        public static DateTime? GetDateTaken(this ExifReader reader)
        {
            if (reader.GetTagValue<DateTime>(ExifTags.DateTime, out var dateTaken))
            {
                return dateTaken;
            }
            else if (reader.GetTagValue(ExifTags.GPSDateStamp, out dateTaken))
            {
                return dateTaken;
            }
            else if (reader.GetTagValue(ExifTags.DateTimeOriginal, out dateTaken))
            {
                return dateTaken;
            }
            else if (reader.GetTagValue(ExifTags.DateTimeDigitized, out dateTaken))
            {
                return dateTaken;
            }

            return null;
        }

        public static double? GetAltitude(this ExifReader reader)
        {
            if (reader.GetTagValue<double>(ExifTags.GPSAltitude, out var altitude))
            {
                return altitude;
            }

            return null;
        }

        public static ushort? GetLength(this ExifReader reader)
        {
            if (reader.GetTagValue<ushort>(ExifTags.ImageLength, out var length))
            {
                return length;
            }

            return null;
        }

        public static ushort? GetWidth(this ExifReader reader)
        {
            if (reader.GetTagValue<ushort>(ExifTags.ImageWidth, out var width))
            {
                return width;
            }

            return null;
        }

        private static double? GetCoordinate(this ExifReader reader, ExifTags type)
        {
            if (reader.GetTagValue(type, out double[] coordinates))
            {
                return ToDoubleCoordinates(coordinates);
            }

            return null;
        }

        private static double ToDoubleCoordinates(double[] coordinates) => coordinates[0] + (coordinates[1] / 60f) + (coordinates[2] / 3600f);
    }
}
