using System;

namespace BungieNetApi
{
    public record Activity
    {
        public long InstanceId { get; set; }

        public DateTime Period { get; set; }

        public ActivityType ActivityType { get; set; }

        public ActivityUserStats[] ActivityUserStats { get; set; }
    }
}
