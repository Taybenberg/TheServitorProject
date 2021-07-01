using Flurl.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace DataProcessor
{
    internal class ImageLoader : IDisposable
    {
        private ConcurrentDictionary<string, Image> cachedImages = new();

        public async Task<Image> GetImage(string url)
        {
            if (!cachedImages.ContainsKey(url))
            {
                try
                {
                    using var stream = await url.GetStreamAsync();
                    cachedImages.TryAdd(url, await Image.LoadAsync(stream));
                }
                catch
                {
                    return new Image<Rgba32>(1, 1);
                }
            }

            return cachedImages[url];
        }
        public void Dispose()
        {
            foreach (var image in cachedImages)
                image.Value.Dispose();

            cachedImages.Clear();
        }
    }
}
