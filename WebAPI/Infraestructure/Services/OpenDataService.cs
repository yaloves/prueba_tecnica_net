using Microsoft.Extensions.Logging;

namespace Infraestructure.Services
{
    public class OpenDataService
    {
        private readonly ILogger<OpenDataService> _logger;
        private readonly HttpClient _httpClient;

        public OpenDataService(ILogger<OpenDataService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<string> GetEndpointAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("GetStringAsync error");
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
