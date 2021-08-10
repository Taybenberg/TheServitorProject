using BungieNetApi.Entities;
using Flurl;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BungieNetApi
{
    public class ApiClient : IApiClient
    {
        private readonly IConfiguration _configuration;

        private readonly BungieNetApiClient _bungieNetApiClient;

        private readonly long _clanId;

        public ApiClient(IConfiguration configuration)
        {
            _configuration = configuration;

            _clanId = configuration.GetSection("Destiny2:ClanID").Get<long>();

            _bungieNetApiClient = new(configuration.GetSection("Destiny2:BungieApiKey").Get<ApiKey>());
        }

        public IEntityFactory EntityFactory
        {
            get
            {
                return new EntityFactory(_bungieNetApiClient);
            }
        }

        public Clan Clan
        {
            get
            {
                return new Clan(_bungieNetApiClient, _clanId);
            }
        }

        public async Task<Nightfall> GetNightfallAsync(long activityHash)
        {
            var nf = new Nightfall();

            var nightfall = await _bungieNetApiClient.getRawActivityDefinitionAsync(activityHash);

            nf.Name = nightfall.originalDisplayProperties.description;
            nf.Type = nightfall.selectionScreenDisplayProperties.name;
            nf.Image = BungieNetApiClient.BUNGIE_NET_URL.AppendPathSegment(nightfall.pgcrImage);

            return nf;
        }

        public async Task<Milestone> GetMilestonesAsync()
        {
            var milestone = new Milestone();

            var rawMilestones = (await _bungieNetApiClient.getRawMilestonesAsync()).Response;

            if (rawMilestones is not null)
            {
                if (rawMilestones.ContainsKey("3312774044"))
                {
                    var rawCrucible = rawMilestones["3312774044"].activities.Where(x => x.activityHash is
                        not 1717505396 //Control
                        and not 1957660400 //Elimination
                        and not 2259621230 //Rumble
                    ).FirstOrDefault();

                    var crucible = await _bungieNetApiClient.getRawActivityDefinitionAsync(rawCrucible.activityHash);

                    milestone.CrucibleRotationModeName = crucible.originalDisplayProperties.name;
                }

                if (rawMilestones.ContainsKey("1942283261"))
                {
                    var nfIDs = _configuration.GetSection("Destiny2:NoMatchmakingNightfalls").Get<HashSet<long>>();

                    var rawNightfall = rawMilestones["1942283261"].activities.Where(x => !nfIDs.Any(y => y == x.activityHash)).FirstOrDefault();

                    var nightfall = await _bungieNetApiClient.getRawActivityDefinitionAsync(rawNightfall.activityHash);

                    milestone.NightfallTheOrdealName = nightfall.originalDisplayProperties.description;
                    milestone.NightfallTheOrdealImage = BungieNetApiClient.BUNGIE_NET_URL.AppendPathSegment(nightfall.pgcrImage);
                }
            }

            return milestone;
        }

        public async Task<IEnumerable<Item>> GetXurItemsAsync()
        {
            var rawXurItems = await _bungieNetApiClient.getRawXurItemsAsync();

            return rawXurItems.saleItems.Values.Skip(1).SkipLast(1).Select(x =>
            new Item(_bungieNetApiClient)
            {
                ItemHash = x.itemHash
            }).Reverse();
        }
    }
}
