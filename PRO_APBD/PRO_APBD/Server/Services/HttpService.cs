using PRO_APBD.Server.DTOs;
using static System.Net.WebRequestMethods;

namespace PRO_APBD.Server.Services;

public interface IHttpService
{

    Task<TickerInfoDto?> GetTickerInfo(string searched);
    Task<TickerAgregateDto?> GetTickerAggregate(string name);
    Task<GeneralInfoDto?> GetGeneralInfo(string name);
    Task<TickerSessionEndDto?> GetSessionEndInfo(string searched);
    Task<StockNewsDto?> GetNews(string searched);




}
public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
    private readonly string API_KEY;
    public HttpService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        API_KEY = configuration.GetValue<string>("PolygonKey")!;
    }

    public async Task<StockNewsDto?> GetNews(string searched)
    {
        try
        {
            var news = await _httpClient.GetFromJsonAsync<StockNewsDto>($"https://api.polygon.io/v2/reference/news?ticker={searched}&order=desc&limit=5&sort=published_utc&apiKey={API_KEY}");
            return news;
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
            return null;
        }
    }

    public async Task<TickerSessionEndDto?> GetSessionEndInfo(string searched)
    {
        var day = DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd");
        try
        {
            var seshend = await _httpClient.GetFromJsonAsync<TickerSessionEndDto>($"https://api.polygon.io/v1/open-close/{searched}/{day}?adjusted=true&apiKey={API_KEY}");
            return seshend;
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
            return null;
        }


    }

    public async Task<TickerInfoDto?> GetTickerInfo(string searched)
    {
        try
        {
            var infos = await _httpClient.GetFromJsonAsync<TickerInfoDto>($"https://api.polygon.io/v3/reference/tickers?search={searched}&active=true&limit=1000&apiKey={API_KEY}");
            return infos;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<TickerAgregateDto?> GetTickerAggregate(string name)
    {
        var from = DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd");
        var to = DateTime.Today.ToString("yyyy-MM-dd");
        await Console.Out.WriteLineAsync(from);

        try
        {
            return await _httpClient.GetFromJsonAsync<TickerAgregateDto?>($"https://api.polygon.io/v2/aggs/ticker/{name}/range/1/day/{from}/{to}?adjusted=true&sort=asc&limit=50000&apiKey={API_KEY}");
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
            return null;

        }
    }


    public async Task<GeneralInfoDto?> GetGeneralInfo(string name)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<GeneralInfoDto?>($"https://api.polygon.io/v3/reference/tickers/{name}?apiKey={API_KEY}");
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
            return null;

        }

    }
}
