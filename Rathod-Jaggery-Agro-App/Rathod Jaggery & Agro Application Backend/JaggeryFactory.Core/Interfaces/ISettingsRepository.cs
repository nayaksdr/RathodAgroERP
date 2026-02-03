using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public interface ISettingsRepository
    {
        Task<decimal> GetDailyRateAsync();
        Task<string> GetValueAsync(string key);
        Task SetValueAsync(string key, string value);
    }
}
