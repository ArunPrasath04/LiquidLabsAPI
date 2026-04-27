using LiquidLabsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LiquidLabsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptoCoinController : ControllerBase
    {
        private readonly ICryptoService _cryptoService;
        public CryptoCoinController(ICryptoService cryptoService) {
            _cryptoService = cryptoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _cryptoService.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _cryptoService.GetById(id);
            return Ok(result);
        }
    }
}
