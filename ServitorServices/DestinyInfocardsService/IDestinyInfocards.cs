using DestinyInfocardsService.Infocards;

namespace DestinyInfocardsService
{
    public interface IDestinyInfocards
    {
        Task<XurInfocard> GetXurInfocardAsync();

        Task<LostSectorsInfocard> GetLostSectorsInfocardAsync();

        Task<EververseInfocard> GetEververseInfocardAsync(int? week = null);
    }
}
