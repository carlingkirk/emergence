using System;
using System.Collections.Generic;
using System.Text;

namespace Emergence.Data.Stores
{
    public class Specimen
    {
        public long SpecimenId { get; set; }
        public long InventoryItemId { get; set; }
        public string SpecimenStage { get; set; }
    }
}
