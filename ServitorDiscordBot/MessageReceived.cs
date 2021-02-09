using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task MessageReceivedAsync(SocketMessage message)
        {
            if (message.Author.Id == _client.CurrentUser.Id || message.Author.IsBot || (message.Channel.Name.ToLower() != "destiny_bot" && message.Channel.Name.ToLower() != "servitor_beta"))
                return;

            if (message.Content == "біп")
                await message.Channel.SendMessageAsync("біп…");
            else if (message.MentionedUsers.Where(x => x.Username == _client.CurrentUser.Username).Count() > 0 && message.Content.ToLower().Contains("привітайся"))
            {
                await message.Channel.SendMessageAsync($"Ах, точно. Я {_client.CurrentUser.Username}, " +
                    $"дружній прислужник, якого на околицях сонячної системи підібрав наш ґардіан. " +
                    $"Я не становлю загрози і присягаюсь служити на благо Останнього міста. " +
                    $"Наразі Авангард надав мені роль обчислювальної машини для збору статистичних даних про діяльність вашого клану.");
            }
        }
    }
}
