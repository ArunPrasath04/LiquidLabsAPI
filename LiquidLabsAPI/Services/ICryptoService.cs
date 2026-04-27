using LiquidLabsAPI.Models;

namespace LiquidLabsAPI.Services
{
    public interface ICryptoService
    {
        Task<CryptoCoin> GetById(string id);
        Task<List<CryptoCoin>> GetAll();
    }
}
