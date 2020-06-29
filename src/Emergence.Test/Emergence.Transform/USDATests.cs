using System.Collections.Generic;
using Emergence.Transform.USDA;
using FluentAssertions;
using Microsoft.VisualBasic;
using Xunit;

namespace Emergence.Test.Emergence.Transform
{
    public class USDATests
    {
        [Theory]
        [MemberData(nameof(GetNames))]
        public void ChecklistParserTest(string scientificNameWithAuthor, string genus, string species, string author, string variant)
        {
            var (Genus, Species, Author, Variant) = ChecklistParser.ParseScientificNameWithAuthor(scientificNameWithAuthor);
            
            Genus.Should().Be(genus);
            Species.Should().Be(species);
            Author.Should().Be(author);
            Variant.Should().Be(variant);
        }

        public static IEnumerable<object[]> GetNames()
        {
            yield return new object[] { "Abies magnifica A. Murray bis var. critchfieldii Lanner", "Abies", "magnifica", "Lanner", "critchfieldii" };
            yield return new object[] { "Hibiscus manihot L.", "Hibiscus", "manihot", "L.", null };
            yield return new object[] { "Lindernia dubia (L.) Pennell var. inundata (Pennell) Pennell", "Lindernia", "dubia", "(Pennell) Pennell", "inundata" };
            yield return new object[] { "Amaranthus ×tucsonensis Henrickson", "Amaranthus", "×tucsonensis", "Henrickson", null };
        }
    }
}
