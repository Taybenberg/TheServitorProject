using Flurl;
using Flurl.Http;
using System;
using System.Threading.Tasks;
using Utf8Json;

namespace BungieNetApi
{
    partial class BungieNetApiClient
    {
        /*
        /// <summary>
        /// Destiny2.GetDestinyEntityDefinition
        /// Path: /Destiny2/Manifest/{entityType}/{hashIdentifier}/
        /// </summary>
        /// <returns>Returns the static definition of an entity of the given Type and hash identifier. Examine the API Documentation for the Type Names of entities that have their own definitions. Note that the return type will always *inherit from* DestinyDefinition, but the specific type returned will be the requested entity type if it can be found. Please don't use this as a chatty alternative to the Manifest database if you require large sets of data, but for simple and one-off accesses this should be handy.</returns>
        */

        public async Task<API.Destiny2.Manifest.DestinyActivityDefinition.Response> getRawActivityDefinitionAsync(long entityHash)
        {
            var entityReequest = API.Destiny2.Url.BaseURL
                .AppendPathSegment("Manifest")
                .AppendPathSegment("DestinyActivityDefinition")
                .AppendPathSegment(entityHash)
                .WithHeader(_xApiKey.Name, _xApiKey.Value);

            API.Destiny2.Manifest.DestinyActivityDefinition.Rootobject result;

            try
            {
                result = await JsonSerializer.DeserializeAsync<API.Destiny2.Manifest.DestinyActivityDefinition.Rootobject>(await entityReequest.GetStreamAsync());
            }
            catch (Exception) //server error (method is in preview)
            {
                return null;
            }

            return result.Response;
        }

        public async Task<API.Destiny2.Manifest.DestinyEntityDefinition.Response> getRawItemDefinitionAsync(long entityHash)
        {
            var entityReequest = API.Destiny2.Url.BaseURL
                .AppendPathSegment("Manifest")
                .AppendPathSegment("DestinyInventoryItemDefinition")
                .AppendPathSegment(entityHash)
                .WithHeader(_xApiKey.Name, _xApiKey.Value);

            API.Destiny2.Manifest.DestinyEntityDefinition.Rootobject result;

            try
            {
                result = await JsonSerializer.DeserializeAsync<API.Destiny2.Manifest.DestinyEntityDefinition.Rootobject>(await entityReequest.GetStreamAsync());
            }
            catch (Exception) //server error (method is in preview)
            {
                return null;
            }

            return result.Response;
        }
    }
}
