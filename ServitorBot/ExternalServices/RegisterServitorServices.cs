using BumperService;
using Microsoft.Extensions.DependencyInjection;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private IBumpManager _bumper;

        private void RegisterExternalServices()
        {
            using var scope = _scopeFactory.CreateScope();

            _bumper = scope.ServiceProvider.GetRequiredService<IBumpManager>();
            _bumper.Notify += BumperNotify;
        }
    }
}
