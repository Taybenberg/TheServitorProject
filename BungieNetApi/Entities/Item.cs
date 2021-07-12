using Flurl;
using System;

namespace BungieNetApi.Entities
{
    public class Item
    {
        public long ItemHash { get; internal set; }

        private class ItemContainer
        {
            public string ItemName;
            public string ItemIconUrl;
            public string ItemTypeAndTier;
            public string UniqueLabel;

            internal ItemContainer(BungieNetApiClient apiClient, long hash)
            {
                var rawItemDetails = apiClient.getRawItemDefinitionAsync(hash).Result;

                if (rawItemDetails is not null)
                {
                    ItemName = rawItemDetails.displayProperties.name;
                    ItemIconUrl = BungieNetApiClient.BUNGIE_NET_URL.AppendPathSegment(rawItemDetails.displayProperties.icon);
                    ItemTypeAndTier = rawItemDetails.itemTypeAndTierDisplayName;
                    UniqueLabel = rawItemDetails.equippingBlock.uniqueLabel;
                }
            }
        }

        private Lazy<ItemContainer> _container;

        private readonly BungieNetApiClient _apiClient;

        internal Item(BungieNetApiClient apiClient)
        {
            _apiClient = apiClient;

            _container = new(() => new ItemContainer(_apiClient, ItemHash));
        }

        public string ItemName
        {
            get
            {
                return _container.Value.ItemName;
            }
        }

        public string ItemIconUrl
        {
            get
            {
                return _container.Value.ItemIconUrl;
            }
        }

        public string ItemTypeAndTier
        {
            get
            {
                return _container.Value.ItemTypeAndTier;
            }
        }

        public string UniqueLabel
        {
            get
            {
                return _container.Value.UniqueLabel;
            }
        }
    }
}
