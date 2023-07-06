using PRO_APBD.Shared.DTOs;
using System.Net.Http.Json;

namespace PRO_APBD.Client.Services
{

    public interface IHttpService
    {
        Task<HttpResponseMessage> LoginUser(UserAuthDto user);
        Task<HttpResponseMessage> RegisterUser(UserRegistrationDto user);

        Task<List<SearchData>> GetSearchStock(string searched);
        Task<List<ChartDataDto>> GetChartData(string searched);
        Task<GeneralStockInfoDto?> GetGeneralInfo(string searched);
        Task<StockSessionEndDto?> GetSessionEnd(string searched);
        Task<List<NewsDto>?> GetNews(string searched);
        Task<List<GeneralStockInfoDto>?> GetSubsFor(string username);
        Task Subscribe(string ticker, string username);
        Task DeleteSubscription(string ticker, string username);

    }
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<NewsDto>?> GetNews(string searched)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<NewsDto>>($"/api/Stocks/{searched}/news");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return null;
            }
        }

        public async Task<StockSessionEndDto?> GetSessionEnd(string searched)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<StockSessionEndDto?>($"/api/Stocks/{searched}/eos");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return null;
            }
        }
        public async Task<GeneralStockInfoDto?> GetGeneralInfo(string searched)
        {
            return await _httpClient.GetFromJsonAsync<GeneralStockInfoDto>($"/api/Stocks/{searched}/general");
        }

        public async Task<List<SearchData>> GetSearchStock(string searched)
        {
            try
            {
                var list = await _httpClient.GetFromJsonAsync<List<SearchData>>($"/api/Stocks/search?searched={searched}");
                if (list == null)
                {
                    await Console.Out.WriteLineAsync("List is null");
                    return new List<SearchData>();
                }
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<SearchData>();
            }
        }

        public async Task<List<ChartDataDto>> GetChartData(string searched)
        {
            try
            {
                var list = await _httpClient.GetFromJsonAsync<List<ChartDataDto>>($"/api/Stocks/charts/{searched}");
                if (list == null)
                {
                    await Console.Out.WriteLineAsync("List null");
                    return new List<ChartDataDto>();
                }
                return list;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return new List<ChartDataDto>();
            }
        }

        public async Task<HttpResponseMessage> LoginUser(UserAuthDto user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", user);
            return response;
        }

        public async Task<HttpResponseMessage> RegisterUser(UserRegistrationDto user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", user);
            return response;
        }

        public async Task<List<GeneralStockInfoDto>?> GetSubsFor(string username)
        {
            return await _httpClient.GetFromJsonAsync<List<GeneralStockInfoDto>>($"api/Stocks/subs/{username}");
        }

        public async Task Subscribe(string ticker, string username)
        {
            var stockUser = new StockUserDto
            {
                Username = username,
                Ticker = ticker
            };
            await _httpClient.PostAsJsonAsync("api/Stocks/subs", stockUser);
        }

        public async Task DeleteSubscription(string ticker, string username)
        {
            await _httpClient.DeleteAsync($"api/Stocks/subs/{username}/{ticker}");
        }
    }
}
