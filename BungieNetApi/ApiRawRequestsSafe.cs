using Flurl;
using Flurl.Http;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace BungieNetApi
{
    public partial class BungieNetApiClient
    {
        /// <summary>
        /// Destiny2.GetPublicMilestones
        /// Path: /Destiny2/Milestones/
        /// </summary>
        /// <returns>Gets public information about currently available Milestones.</returns>
        private async Task<API.Destiny2.GetPublicMilestones.Rootobject> getRawMilestonesAsync()
        {
            var milestoneRequest = API.Destiny2.Url.BaseURL
                .AppendPathSegment("Milestones")
                .WithHeader(_xApiKey.Name, _xApiKey.Value);

            try
            {
                return await JsonSerializer.DeserializeAsync<API.Destiny2.GetPublicMilestones.Rootobject>(await milestoneRequest.GetStreamAsync());
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
