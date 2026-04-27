using LiquidLabsAPI.Models;

namespace LiquidLabsAPI.DataAccess
{
    public interface ICryptoCoinRepository
    {
        Task<List<CryptoCoin>> GetLocalCoinsAsync();
        Task AddCoinsAsync(List<CryptoCoin> coins);
        Task<CryptoCoin> GetLocalCoinByIdAsync(string id);
        Task<bool> CheckCoinsExistsAsync();
    }
}
