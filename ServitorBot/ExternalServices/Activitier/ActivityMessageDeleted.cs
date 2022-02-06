namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private async Task ActivityMessageDeletedAsync(ulong messageID)
        {
            await _activityManager.DisableActivityAsync(messageID);
        }
    }
}