﻿//using DataProcessor.Parsers;
//using System;
//using System.IO;
//using System.Threading.Tasks;

//namespace DataProcessor
//{
//    public class ImageFactory : IImageFactory
//    {
//        private readonly IParserFactory _factory;

//        public ImageFactory(IParserFactory factory) => _factory = factory;

//        public async Task<Stream> GetDailyResetAsync(string seasonName, int weekNumber)
//        {
//            var parser = _factory.GetDailyResetParser(seasonName, weekNumber);

//            return await parser.GetImageAsync();
//        }

//        public async Task<Stream> GetWeeklyResetAsync(string seasonName, DateTime seasonStart, int weekNumber)
//        {
//            var parser = _factory.GetWeeklyResetParser(seasonName, seasonStart, weekNumber);

//            return await parser.GetImageAsync();
//        }

//        public async Task<Stream> GetEververseAsync(string seasonName, DateTime seasonStart, int weekNumber)
//        {
//            var parser = _factory.GetEververseParser(seasonName, seasonStart, weekNumber);

//            return await parser.GetImageAsync();
//        }

//        public async Task<Stream> GetEververseFullAsync(string seasonName, DateTime seasonStart, DateTime seasonEnd)
//        {
//            var parser = _factory.GetEververseParser(seasonName, seasonStart, -1) as EververseParser;

//            return await parser.GetFullInventoryAsync(seasonEnd);
//        }

//        public async Task<Stream> GetLostSectorsAsync()
//        {
//            var parser = _factory.GetLostSectorsParser();

//            return await parser.GetImageAsync();
//        }

//        public async Task<Stream> GetResourcesAsync()
//        {
//            var parser = _factory.GetResourcesParser();

//            return await parser.GetImageAsync();
//        }

//        public async Task<Stream> GetXurAsync(bool getLocation)
//        {
//            var parser = _factory.GetXurParser(getLocation);

//            return await parser.GetImageAsync();
//        }
//    }
//}
