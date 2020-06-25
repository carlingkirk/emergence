using Emergence.Transform.USDA;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Emergence.Test.Emergence.Transform
{
    public class USDATests
    {
        [Theory]
        [MemberData(nameof(GetNames))]
        public void ChecklistParserTest(string scientificNameWithAuthor, (string Genus, string Species, string Author, string Variant) expected)
        {
            var actual = ChecklistParser.ParseScientificNameWithAuthor(scientificNameWithAuthor);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> GetNames()
        {
            yield return new object[] { "Abies magnifica A. Murray bis var. critchfieldii Lanner", ("Abies", "magnifica", "Lanner", "critchfieldii") };
        }
    }
}
