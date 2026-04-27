using LiquidLabsAPI.DataAccess;
using LiquidLabsAPI.Models;

namespace LiquidLabsAPI.Services
{
    public class CryptoService : ICryptoService
    {
        private readonly HttpClient _httpClient;
        private readonly ICryptoCoinRepository _cryptoCoinRepository;
        public CryptoService(HttpClient httpClient, ICryptoCoinRepository cryptoCoinRepository)
        {
            _httpClient = httpClient;
            _cryptoCoinRepository = cryptoCoinRepository;
        }

        public async Task<List<CryptoCoin>> GetAll()
        {
            try
            {
                bool exists = await _cryptoCoinRepository.CheckCoinsExistsAsync();

                if (!exists)
                {
                    _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; LiquidLabsAPI/1.0)");
                    var response = await _httpClient.GetFromJsonAsync<List<CryptoCoin>>("https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd");

                    if (response != null && response.Any())
                    {
                        await _cryptoCoinRepository.AddCoinsAsync(response);
                    }
                }

                return await _cryptoCoinRepository.GetLocalCoinsAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CryptoCoin> GetById(string id)
        {
            try
            {
                bool exists = await _cryptoCoinRepository.CheckCoinsExistsAsync();

                if (!exists)
                {
                    var response = await _httpClient.GetFromJsonAsync<List<CryptoCoin>>("https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd");

                    if (response != null && response.Any())
                    {
                        await _cryptoCoinRepository.AddCoinsAsync(response);
                    }
                }

                return await _cryptoCoinRepository.GetLocalCoinByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
