using Flurl.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessor
{
    internal class ImageLoader
    {
        private static string cachePath;

        private static ConcurrentBag<string> cachedImages;

        static ImageLoader()
        {
            cachePath = Path.Combine(Path.GetTempPath(), "ServitorBotTemp");

            if (!Directory.Exists(cachePath))
                Directory.CreateDirectory(cachePath);

            cachedImages = new(Directory.GetFiles(cachePath));
        }

        public async Task<Image> GetImageAsync(string url)
        {
            string filename = Path.Combine(cachePath, Path.GetFileName(url));

            if (!cachedImages.Contains(filename))
            {
                try
                {
                    var bytes = await url.GetBytesAsync();

                    await File.WriteAllBytesAsync(filename, bytes);

                    cachedImages.Add(filename);
                }
                catch
                {
                    return new Image<Rgba32>(1, 1);
                }
            }

            return await Image.LoadAsync(filename);
        }
    }
}
