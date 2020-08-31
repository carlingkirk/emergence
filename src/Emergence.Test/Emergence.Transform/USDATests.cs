using System.Collections.Generic;
using Emergence.Transform.USDA;
using FluentAssertions;
using Xunit;

namespace Emergence.Test.Emergence.Transform
{
    public class USDATests
    {
        [Theory]
        [MemberData(nameof(GetNames))]
        public void ChecklistParserTest(string scientificNameWithAuthor, string expectedGenus, string expectedSpecies, string expectedAuthor,
            string expectedSubspecies, string expectedVariety, string expectedSubvariety)
        {
            var (genus, species, author, subspecies, variety, subvariety) = ChecklistParser.ParseScientificNameWithAuthor(scientificNameWithAuthor);

            genus.Should().Be(expectedGenus);
            species.Should().Be(expectedSpecies);
            author.Should().Be(expectedAuthor);
            subspecies.Should().Be(expectedSubspecies);
            variety.Should().Be(expectedVariety);
            subvariety.Should().Be(expectedSubvariety);
        }

        public static IEnumerable<object[]> GetNames()
        {
            yield return new object[] { "Abies magnifica A. Murray bis var. critchfieldii Lanner",
                "Abies", "magnifica", "Lanner", null, "critchfieldii", null };
            yield return new object[] { "Hibiscus manihot L.",
                "Hibiscus", "manihot", "L.", null, null, null };
            yield return new object[] { "Lindernia dubia (L.) Pennell var. inundata (Pennell) Pennell",
                "Lindernia", "dubia", "(Pennell) Pennell", null, "inundata", null };
            yield return new object[] { "Amaranthus ×tucsonensis Henrickson",
                "Amaranthus", "×tucsonensis", "Henrickson", null, null, null };
            yield return new object[] { "Ruellia caroliniensis (J.F. Gmel.) Steud. ssp. ciliosa (Pursh) R.W. Long var. cinerascens (Fernald) Kartesz & Gandhi",
                "Ruellia", "caroliniensis", "(Fernald) Kartesz & Gandhi", "ciliosa", "cinerascens", null };
            yield return new object[] { "Anatherum virginicum (L.) Spreng. subvar. mohrii (Hack.) Roberty",
                "Anatherum", "virginicum", "(Hack.) Roberty", null, null, "mohrii" };
            yield return new object[] { "Abelmoschus esculentus (L.) Moench",
                "Abelmoschus", "esculentus", "(L.) Moench", null, null, null };
            yield return new object[] { "Acaulon Müll. Hal.", "Acaulon", null, "Müll. Hal.", null, null, null };
            yield return new object[] { "Nodobryoria Common & Brodo", "Nodobryoria", null, "Common & Brodo", null, null, null };
            yield return new object[] { "Acamptopappus (A. Gray) A. Gray", "Acamptopappus", null, "(A. Gray) A. Gray", null, null, null };
        }
    }
}
