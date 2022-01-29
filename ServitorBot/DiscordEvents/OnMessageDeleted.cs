﻿namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task OnMessageDeletedAsync(ulong messageID, ulong channelID)
        {
            if (_activityChannelIDs.Any(x => x == channelID))
                await ActivityMessageDeletedAsync(messageID);
        }
    }
}
