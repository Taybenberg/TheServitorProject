using DestinyInfocardsService.Infocards;

namespace DestinyInfocardsService
{
    public partial class DestinyInfocardsManager
    {
        public async Task<XurInfocard> GetXurInfocardAsync()
        {
            (var resetBegin, var resetEnd) = GetWeeklyResetInterval();

            

            return new XurInfocard();
        }
    }
}
