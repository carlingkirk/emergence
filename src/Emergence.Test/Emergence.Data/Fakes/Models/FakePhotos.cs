using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Models;

namespace Emergence.Test.Data.Fakes.Models
{
    public static class FakePhotos
    {
        public static IEnumerable<Photo> Get()
        {
            var photos = Stores.FakePhotos.Get().Select(p => p.AsModel("https://blobs.com/photos/"));
            return photos;
        }
    }
}
