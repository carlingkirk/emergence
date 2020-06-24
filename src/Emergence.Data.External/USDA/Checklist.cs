using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper.Configuration.Attributes;

namespace Emergence.Data.External.USDA
{
    public class Checklist
    {
        public string Symbol { get; set; }
        [Name("Synonym Symbol")]
        public string SynonymSymbol { get; set; }
        [Name("Scientific Name with Author")]
        public string ScientificNameWithAuthor { get; set; }
        [Name("Common Name")]
        public string CommonName { get; set; }
        public string Family { get; set; }
    }
}
