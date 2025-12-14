using Domain_Layer.Contract.Cash;
using Service_Abstraction.Interfaces;
using System.Text.Json;

namespace Service_Implemention.Service
{
    public class CasService(ICashRepo _cashRepo) : ICashService
    {
        public async Task<string?> GetAsync(string key) => await _cashRepo.GetCashedDataAsync(key);

        public async Task SetAsync(string key, object value, TimeSpan TimeToLive)
        {
            var CashValue = JsonSerializer.Serialize(value);
            await _cashRepo.SetDataAsync(key, CashValue, TimeToLive);
        }
    }
}
