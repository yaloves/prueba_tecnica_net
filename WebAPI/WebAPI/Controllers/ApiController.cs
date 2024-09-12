using Core.Models;
using Infraestructure.Services;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using Logic;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController(ILogger<ApiController> logger, ApiManager apiManager) : ControllerBase
    {
        private readonly ILogger<ApiController> _logger = logger;
        private readonly ApiManager _apiManager = apiManager;

        [HttpGet("CopyData")]
        public async Task<bool> CopyData()
        {
            return await _apiManager.CopyExternalApiToDbAsync();
        }

        [HttpGet("GetBanksById/{id}")]
        public async Task<IResult> GetBanksById(string id)
        {
            var result = await _apiManager.GetBanksByIdAsync(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        }

        [HttpGet("GetAllBanks")]
        public async Task<IResult> GetAllBanks()
        {
            var result = await _apiManager.GetBanksAsync();
            return result != null ? Results.Ok(result) : Results.NotFound();
        }
    }
}
