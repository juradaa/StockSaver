using Microsoft.EntityFrameworkCore;
using PRO_APBD.Server.DTOs;
using PRO_APBD.Server.Models;
using PRO_APBD.Shared.DTOs;

namespace PRO_APBD.Server.Repositories;

public interface IStocksRepository
{
    Task<bool> StockExists(string ticker);
    Task AddStock(Stock stock);
    Task UpdateStock(string ticker, GeneralInfoDto newData);
    Task<Stock?> GetStock(string ticker);
    Task<List<Hloc>> GetHlocs(string ticker);
    Task UpdateHlocks(string ticker, List<Hloc> newData);
    Task<SessionEnd?> GetSessionEnd(string ticker);
    Task UpdateSessionEnd(string ticker, StockSessionEndDto session);
    Task<List<Models.News>> GetNews(string ticker);
    Task UpdateNews(string ticker, List<News> newData);
    Task<bool> ExistsUserStock(string ticker, string username);
    Task AddUserStock(UserStock userStock);
    Task<List<Stock>> GetStocksForUser(string username);
    Task DeleteUserStockIfExists(string username, string ticker);
}

public class StocksRepository : IStocksRepository
{
    private readonly DataContext _context;

    public StocksRepository(DataContext context)
    {
        _context = context;
    }

    public async Task AddUserStock(UserStock userStock)
    {
        _context.UserStocks.Add(userStock);
        await _context.SaveChangesAsync();
    }

    public async Task AddStock(Stock stock)
    {
        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsUserStock(string ticker, string username)
    {
        return await _context.UserStocks.AnyAsync(us=> us.Stock.Ticker == ticker && us.User.Username == username);
    }

    public async Task<List<Hloc>> GetHlocs(string ticker)
    {
        return await _context.Hlocs.Where(h => h.Stock.Ticker == ticker).ToListAsync();
    }

    public async Task<List<News>> GetNews(string ticker)
    {
        return await _context.News.Where(n => n.Stock.Ticker == ticker).ToListAsync();
    }

    public async Task<SessionEnd?> GetSessionEnd(string ticker)
    {
        return await _context.SessionEnds.FirstOrDefaultAsync(se => se.Stock.Ticker == ticker);
    }

    public async Task<Stock?> GetStock(string ticker)
    {
        await Console.Out.WriteLineAsync("Getting");
        return await _context.Stocks.FirstOrDefaultAsync(stock => stock.Ticker == ticker);
    }

    public async Task<List<Stock>> GetStocksForUser(string username)
    {
        return await _context.Stocks.Where(s=>s.UserStocks.Any(us=>us.User.Username == username)).ToListAsync();    
    }

    public async Task<bool> StockExists(string ticker)
    {
        return await _context.Stocks.AnyAsync(stock => stock.Ticker == ticker);
    }

    public async Task UpdateHlocks(string ticker, List<Hloc> newData)
    {
        var stock = await _context.Stocks.FirstOrDefaultAsync(st => st.Ticker == ticker);
        if (stock != null)
        {
            var toRemove = await _context.Hlocs.Where(h => h.IdStock == stock.Id).ToListAsync();
            _context.Hlocs.RemoveRange(toRemove);
            await _context.SaveChangesAsync();
            newData.ForEach(nd => nd.Stock = stock);
            _context.Hlocs.AddRange(newData);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateNews(string ticker, List<News> newData)
    {
        var stock = await _context.Stocks.FirstOrDefaultAsync(st => st.Ticker == ticker);
        if (stock != null)
        {
            var toRemove = await _context.News.Where(n => n.Stock.Id == stock.Id).ToListAsync();
            _context.News.RemoveRange(toRemove);
            await _context.SaveChangesAsync();
            newData.ForEach(nd => nd.Stock = stock);
            _context.News.AddRange(newData);
            await _context.SaveChangesAsync();
        }

    }

    public async Task UpdateSessionEnd(string ticker, StockSessionEndDto session)
    {
        var sessionDb = await _context.SessionEnds.FirstOrDefaultAsync(se => se.Stock.Ticker == ticker);
        if (sessionDb == null)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(st => st.Ticker == ticker);
            
            if(stock is null)
            {
                return;
            }

            var newSesh = new SessionEnd
            {
                From = session.From,
                Close = session.Close,
                High = session.High,
                Low = session.Low,
                Volume = session.Volume,
                AfterHours = session.AfterHours,
                PreMarket = session.PreMarket,
                Stock = stock
            };
            _context.SessionEnds.Add(newSesh);
            await _context.SaveChangesAsync();

            return;
        }
        sessionDb.From = session.From;
        sessionDb.Open = session.Open;
        sessionDb.Close = session.Close;
        sessionDb.High = session.High;
        sessionDb.Low = session.Low;
        sessionDb.Volume = session.Volume;
        sessionDb.AfterHours = session.AfterHours;
        sessionDb.PreMarket = session.PreMarket;
        await _context.SaveChangesAsync();

    }

    public async Task UpdateStock(string ticker, GeneralInfoDto newData)
    {
        await Console.Out.WriteLineAsync("updating");
        var stock = await _context.Stocks.FirstOrDefaultAsync(st => st.Ticker == ticker);
        if (stock == null)
        {
            return;
        }
        stock.Name = newData.results.name;
        stock.Country = newData.results.locale;
        stock.ImageUrl = newData.results.branding.logo_url;
        stock.HomepageUrl = newData.results.homepage_url;
        stock.Currency = newData.results.currency_name;
        await _context.SaveChangesAsync();

    }

    public async Task DeleteUserStockIfExists(string username, string ticker)
    {
        var userStock = await  _context.UserStocks.FirstOrDefaultAsync(us=> us.Stock.Ticker == ticker && us.User.Username == username);
        if (userStock == null)
        {
            return;
        }
        _context.UserStocks.Remove(userStock);
        await _context.SaveChangesAsync();

    }
}
