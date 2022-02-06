﻿using Discord;

namespace ServitorBot.BotCommands.TextCommands
{
    internal partial class ServiceCommandsManager
    {
        private Embed[] HelpEmbeds =>
            new Embed[]
            {
                new EmbedBuilder()
                    .WithColor(0xBE5BEF)
                    .WithTitle("Службові команди")
                    .WithDescription("**!donate** - підтримати розробника бота.\n")
                    .Build(),

                new EmbedBuilder()
                    .WithColor(0xBE5BEF)
                    .WithTitle("Рандом")
                    .WithDescription("Наявні 2 типи команд:\n" +
                            "**!random** ***N*** – генерує випадкове ціле число X в діапазоні 0 <= X < N.\n" +
                            "**!random** ***@Role*** – обирає випадкового користувача за вказаною роллю.")
                    .Build(),

                new EmbedBuilder()
                    .WithColor(0xBE5BEF)
                    .WithTitle("Видалення повідомлень")
                    .WithDescription("Щоби видалити повідомлення вам необхідно надіслати у відповідь до потрібного повідомлення одну з наступних команд:" +
                            "\n\n**!delete_this** – видаляє вказане повідомлення." +
                            "\n\n**!delete_after** ***count*** – масово видаляє вказану кількість повідомлень ПІСЛЯ вказаного повідомлення. " +
                            "Можливе видалення лише тих повідомлень, які створені менше 2 тижнів тому." +
                            "\n\n**!delete_slow_after** ***count*** – почергово видаляє вказану кількість повідомлень ПІСЛЯ вказаного повідомлення." +
                            "\n\n**!delete_before** ***count*** – масово видаляє вказану кількість повідомлень ДО вказаного повідомлення. " +
                            "Можливе видалення лише тих повідомлень, які створені менше 2 тижнів тому." +
                            "\n\n**!delete_slow_before** ***count*** – почергово видаляє вказану кількість повідомлень ДО вказаного повідомлення." +
                            "\n\nЯкщо вказана кількість перевищує наявну кількість повідомлень у каналі, то буде видалено лише наявні повідомлення у заданому напрямку.\n" +
                            "Почергове видалення не має обмежень по тривалості існування повідомлень, але відбувається дуже повільно через обмеження дискорду.\n" +
                            "Використовувати команду можуть лише користувачі, які мають права на видалення повідомлень на сервері.")
                    .Build()
            };
    }
}
