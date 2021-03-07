using System;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Utf8Json;

namespace BungieNetApi
{
    public partial class BungieNetApiClient
    {
        /// <summary>
        /// Destiny2.GetDestinyEntityDefinition
        /// Path: /Destiny2/Manifest/{entityType}/{hashIdentifier}/
        /// </summary>
        /// <returns>Returns the static definition of an entity of the given Type and hash identifier. Examine the API Documentation for the Type Names of entities that have their own definitions. Note that the return type will always *inherit from* DestinyDefinition, but the specific type returned will be the requested entity type if it can be found. Please don't use this as a chatty alternative to the Manifest database if you require large sets of data, but for simple and one-off accesses this should be handy.</returns>
        private async Task<API.Destiny2.GetDestinyEntityDefinition.Response> GetItemDetailsAsync(string itemHash)
        {
            var itemRequest = API.Destiny2.Url.BaseURL
                .AppendPathSegment("Manifest")
                .AppendPathSegment("DestinyInventoryItemDefinition")
                .AppendPathSegment(itemHash)
                .WithHeader(_xApiKey.Name, _xApiKey.Value);

            API.Destiny2.GetDestinyEntityDefinition.Rootobject result;

            try
            {
                result = await JsonSerializer.DeserializeAsync<API.Destiny2.GetDestinyEntityDefinition.Rootobject>(await itemRequest.GetStreamAsync());
            }
            catch (Exception) //server error (method is in preview)
            {
                return null;
            }

            return result.Response;
        }

        /// <summary>
        /// Destiny2.GetVendors
        /// Path: /Destiny2/{membershipType}/Profile/{destinyMembershipId}/Character/{characterId}/Vendors/
        /// </summary>
        /// <returns>Get currently available vendors from the list of vendors that can possibly have rotating inventory. Note that this does not include things like preview vendors and vendors-as-kiosks, neither of whom have rotating/dynamic inventories. Use their definitions as-is for those.</returns>
        private async Task<API.Destiny2.DestinyComponentType.Components402.Vendor> GetRawXurItemsAsync()
        {
            var xurRequest = API.Destiny2.Url.BaseURL
                .AppendPathSegment("Vendors")
                .SetQueryParam("components", 402)
                .WithHeader(_xApiKey.Name, _xApiKey.Value);

            var result = await JsonSerializer.DeserializeAsync<API.Destiny2.DestinyComponentType.Components402.Rootobject>(await xurRequest.GetStreamAsync());

            return result.Response.sales.data["2190858386"];
        }

        /// <summary>
        /// GroupV2.GetGroupsForMember
        /// Path: /GroupV2/User/{membershipType}/{membershipId}/{filter}/{groupType}/
        /// </summary>
        /// <returns>Get information about the groups that a given member has joined.</returns>
        private async Task<API.GroupV2.GetGroupsForMember.Result[]> getRawUserClansAsync(int membershipType, string membershipId)
        {
            var groupsRequest = API.GroupV2.Url.BaseURL
                .AppendPathSegment("User")
                .AppendPathSegment(membershipType)
                .AppendPathSegment(membershipId)
                .AppendPathSegments("0", "1")
                .WithHeader(_xApiKey.Name, _xApiKey.Value);

            var result = await JsonSerializer.DeserializeAsync<API.GroupV2.GetGroupsForMember.Rootobject>(await groupsRequest.GetStreamAsync());

            return result.Response.results;
        }

        /// <summary>
        /// Destiny2.GetPostGameCarnageReport
        /// Path: /Destiny2/Stats/PostGameCarnageReport/{activityId}
        /// </summary>
        /// <returns>Gets the available post game carnage report for the activity ID.</returns>
        private async Task<API.Destiny2.GetPostGameCarnageReport.Response> getRawActivityDetailsAsync(string activityId)
        {
            var activityRequest = API.Destiny2.Url.PostGameCarnageReportURL
                .AppendPathSegment(activityId)
                .WithHeader(_xApiKey.Name, _xApiKey.Value);

            API.Destiny2.GetPostGameCarnageReport.Rootobject result;

            try
            {
                result = await JsonSerializer.DeserializeAsync<API.Destiny2.GetPostGameCarnageReport.Rootobject>(await activityRequest.GetStreamAsync());
            }
            catch (Exception) //server error (activity may be too old)
            {
                return null;
            }

            return result.Response;
        }

        /// <summary>
        /// Destiny2.GetActivityHistory
        /// Path: /Destiny2/{membershipType}/Account/{destinyMembershipId}/Character/{characterId}/Stats/Activities/
        /// </summary>
        /// <returns>Gets activity history stats for indicated character.</returns>
        private async Task<API.Destiny2.GetActivityHistory.Activity[]> getRawActivitiesAsync(int membershipType, string membershipId, string characterId, int count, int page)
        {
            var activitiesRequest = API.Destiny2.Url.BaseURL
                .AppendPathSegment(membershipType)
                .AppendPathSegment("Account")
                .AppendPathSegment(membershipId)
                .AppendPathSegment("Character")
                .AppendPathSegment(characterId)
                .AppendPathSegments("Stats", "Activities")
                .SetQueryParam("count", count)
                .SetQueryParam("page", page)
                .WithHeader(_xApiKey.Name, _xApiKey.Value);

            API.Destiny2.GetActivityHistory.Rootobject result;

            try
            {
                result = await JsonSerializer.DeserializeAsync<API.Destiny2.GetActivityHistory.Rootobject>(await activitiesRequest.GetStreamAsync());
            }
            catch (Exception) //private account
            {
                return null;
            }

            return result.Response.activities;
        }

        /// <summary>
        /// Destiny2.GetCharacter
        /// Path: /Destiny2/{membershipType}/Profile/{destinyMembershipId}/Character/{characterId}/
        /// </summary>
        /// <returns>Returns character information for the supplied character.</returns>
        private async Task<API.Destiny2.DestinyComponentType.Components200.Character> getRawCharacterAsync(int membershipType, string membershipId, string characterId)
        {
            var characterRequest = API.Destiny2.Url.BaseURL
                .AppendPathSegment(membershipType)
                .AppendPathSegment("Profile")
                .AppendPathSegment(membershipId)
                .AppendPathSegment("Character")
                .AppendPathSegment(characterId)
                .SetQueryParam("components", 200)
                .WithHeader(_xApiKey.Name, _xApiKey.Value);

            var result = await JsonSerializer.DeserializeAsync<API.Destiny2.DestinyComponentType.Components200.Rootobject>(await characterRequest.GetStreamAsync());

            return result.Response.character;
        }

        /// <summary>
        /// Destiny2.GetProfile
        /// Path: /Destiny2/{membershipType}/Profile/{destinyMembershipId}/
        /// </summary>
        /// <returns>Returns Destiny Profile information for the supplied membership.</returns>
        private async Task<API.Destiny2.DestinyComponentType.Components100.Profile> getRawProfileAsync(int membershipType, string membershipId)
        {
            var profileRequest = API.Destiny2.Url.BaseURL
                .AppendPathSegment(membershipType)
                .AppendPathSegment("Profile")
                .AppendPathSegment(membershipId)
                .SetQueryParam("components", 100)
                .WithHeader(_xApiKey.Name, _xApiKey.Value);

            var result = await JsonSerializer.DeserializeAsync<API.Destiny2.DestinyComponentType.Components100.Rootobject>(await profileRequest.GetStreamAsync());

            return result.Response.profile;
        }

        /// <summary>
        /// GroupV2.GetMembersOfGroup
        /// Path: /GroupV2/{groupId}/Members/
        /// </summary>
        /// <returns>Get the list of members in a given group.</returns>
        private async Task<API.GroupV2.GetMembersOfGroup.Result[]> getRawUsersAsync(string clanID)
        {
            var membersRequest = API.GroupV2.Url.BaseURL
                .AppendPathSegment(clanID)
                .AppendPathSegment("members")
                .WithHeader(_xApiKey.Name, _xApiKey.Value);

            var result = await JsonSerializer.DeserializeAsync<API.GroupV2.GetMembersOfGroup.Rootobject>(await membersRequest.GetStreamAsync());

            return result.Response.results;
        }
    }
}
