using Flurl.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Concurrent;

namespace DestinyInfocardsService
{
    internal static class ImageLoader
    {
        private static readonly string _cachePath;

        private static readonly ConcurrentBag<string> _cachedImages;

        static ImageLoader()
        {
            _cachePath = Path.Combine(Path.GetTempPath(), "ServitorBotTemp");

            if (!Directory.Exists(_cachePath))
                Directory.CreateDirectory(_cachePath);

            _cachedImages = new(Directory.GetFiles(_cachePath));
        }

        public static async Task<Image> GetImageAsync(string url)
        {
            string filename = Path.Combine(_cachePath, Path.GetFileName(url));

            if (!_cachedImages.Contains(filename))
            {
                try
                {
                    var bytes = await url.GetBytesAsync();

                    await File.WriteAllBytesAsync(filename, bytes);

                    _cachedImages.Add(filename);
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
