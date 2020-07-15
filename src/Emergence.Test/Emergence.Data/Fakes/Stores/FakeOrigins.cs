using System;
using System.Collections.Generic;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakeOrigins
    {
        public static IEnumerable<Origin> Get() => new List<Origin>
        {
            new Origin
            {
                Id = 1,
                Name = "Plant Finder - Missouri Botanical Garden",
                Description = "Look up, view a photo and read about the over 7,500 plants which are growing or have been grown in the Kemper Center display " +
                "gardens (plus selected additions) by scientific name, common name and/or selected plant characteristics.",
                Type = "Website",
                Uri = new Uri("http://www.missouribotanicalgarden.org/plantfinder/plantfindersearch.aspx")
            },
            new Origin
            {
                Id = 2,
                ParentId = 3,
                Type = "Nursery",
                Name = "Botany Yards",
                Description = ""
            },
            new Origin
            {
                Id = 3,
                Type = "Event",
                Name = "GNPS Symposium 2020",
                Description = "Our 25th annual Symposium is all about connections between native plants and the fauna that rely upon them. Inspired by the " +
                "February book release of Nature’s Best Hope: A New Approach to Conservation that Starts in Your Yard from Doug Tallamy, we invite you to " +
                "be among the first to hear his new message. Growing more native plants to sustain all of nature is more urgent than ever.",
                Uri = new Uri("https://gnps.org/2020-georgia-native-plant-society-annual-symposium/")
            }
        };
    }
}
