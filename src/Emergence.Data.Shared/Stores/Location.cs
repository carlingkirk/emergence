using System;
using System.ComponentModel.DataAnnotations;

namespace Emergence.Data.Shared.Stores
{
    public class Location
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string AddressLine1 { get; set; }
        [StringLength(200)]
        public string AddressLine2 { get; set; }
        [StringLength(200)]
        public string City { get; set; }
        [StringLength(10)]
        public string StateOrProvince { get; set; }
        [StringLength(10)]
        public string PostalCode { get; set; }
        [StringLength(100)]
        public string Country { get; set; }
        [StringLength(100)]
        public string Region { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public double? Altitude { get; set; }
        [StringLength(36)]
        public string CreatedBy { get; set; }
        [StringLength(36)]
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
