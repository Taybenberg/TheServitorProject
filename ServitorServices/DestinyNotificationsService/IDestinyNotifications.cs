using DestinyNotificationsService.Infocards;

namespace DestinyNotificationsService
{
    public interface IDestinyNotifications
    {
        Task<XurInfocard> GetXurInfocardAsync();

        Task<LostSectorsInfocard> GetLostSectorsInfocardAsync();

        Task<EververseInfocard> GetEververseInfocardAsync();
    }
}
