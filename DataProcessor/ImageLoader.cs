using Flurl.Http;
using NetVips;
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
                    return Image.Black(1, 1);
                }
            }

            return Image.NewFromFile(filename);
        }

        public static Image RenderText(string text, string font, int[] textColor, Enums.Align? align = null)
        {
            using var textMask = Image.Text(text, font, align: align);

            var textImage = textMask
                .NewFromImage(textColor)
                .Copy(interpretation: Enums.Interpretation.Srgb)
                .Bandjoin(textMask);

            return textImage;
        }
    }
}
