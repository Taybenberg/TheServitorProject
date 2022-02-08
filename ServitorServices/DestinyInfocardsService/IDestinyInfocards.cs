using DestinyInfocardsService.Infocards;

namespace DestinyInfocardsService
{
    public interface IDestinyInfocards
    {
        Task<XurInfocard> GetXurInfocardAsync();

        Task<LostSectorsInfocard> GetLostSectorsInfocardAsync();

        Task<ResourcesInfocard> GetResourcesInfocardAsync();

        Task<EververseInfocard> GetEververseInfocardAsync(int? week = null);

        Task<WeeklyMilestoneInfocard> GetWeeklyMilestoneInfocardAsync();
    }
}
