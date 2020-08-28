using System;
using System.Collections.Generic;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Test.Data.Fakes.Models
{
    public static class FakeOrigins
    {
        public static IEnumerable<Origin> Get() => new List<Origin>
        {
            new Origin
            {
                OriginId = 1,
                Name = "Plant Finder - Missouri Botanical Garden",
                Description = "Look up, view a photo and read about the over 7,500 plants which are growing or have been grown in the Kemper Center display " +
                "gardens (plus selected additions) by scientific name, common name and/or selected plant characteristics.",
                Type = OriginType.Website,
                Uri = new Uri("http://www.missouribotanicalgarden.org/plantfinder/plantfindersearch.aspx")
            },
            new Origin
            {
                OriginId = 2,
                Type = OriginType.Nursery,
                Name = "Botany Yards",
                Description = ""
            },
            new Origin
            {
                OriginId = 3,
                Type = OriginType.Event,
                Name = "GNPS Symposium 2020",
                Description = "Our 25th annual Symposium is all about connections between native plants and the fauna that rely upon them. Inspired by the " +
                "February book release of Nature’s Best Hope: A New Approach to Conservation that Starts in Your Yard from Doug Tallamy, we invite you to " +
                "be among the first to hear his new message. Growing more native plants to sustain all of nature is more urgent than ever.",
                Uri = new Uri("https://gnps.org/2020-georgia-native-plant-society-annual-symposium/")
            }
        };
    }
}
