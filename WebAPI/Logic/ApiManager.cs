using Core.Models;
using Infraestructure.Data;
using Infraestructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Logic
{
    public class ApiManager(ILogger<ApiManager> logger, ApiDbContext context, OpenDataService openDataService)
    {
        private readonly ILogger<ApiManager> _logger = logger;
        private readonly ApiDbContext _context = context;
        private readonly OpenDataService _openDataService = openDataService;

        public async Task<bool> CopyExternalApiToDbAsync()
        {
            var data = await _openDataService.GetEndpointAsync("https://api.opendata.esett.com/EXP06/Banks");
            if (string.IsNullOrWhiteSpace(data))
            {
                _logger.LogInformation("No data");
                return false;
            }

            var banks = JsonSerializer.Deserialize<List<BankEntity>>(data);
            
            _logger.LogInformation($"Founded {banks.Count}. Cleaning duplicates");
            var existingBanks = await _context.Banks.Select(x => x.Bic).ToListAsync();
            var dataToAdd = banks.Where(x => !existingBanks.Contains(x.Bic)).ToList();

            if (dataToAdd.Count > 0)
            {
                _logger.LogInformation($"Finally add {dataToAdd.Count} records");
                _context.Banks.AddRange(dataToAdd);
                return _context.SaveChanges() > 0;
            }

            _logger.LogInformation("No data to add");
            return false;
            
        }

        public async Task<List<BankEntity>?> GetBanksAsync()
        {
            return await _context.Banks.ToListAsync();
        }

        public async Task<BankEntity?> GetBanksByIdAsync(string id)
        {
            return await _context.Banks.FirstOrDefaultAsync(x => x.Bic == id);
        }
    }
}
