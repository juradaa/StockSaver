using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRO_APBD.Server.Models;
using PRO_APBD.Server.Repositories;
using PRO_APBD.Server.Services;
using PRO_APBD.Shared.DTOs;

namespace PRO_APBD.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StocksController : ControllerBase
    {
        private readonly IHttpService _httpService;
        private readonly IStocksRepository _stocksRepository;
        private readonly IUsersRepository _usersRepository;

        public StocksController(IHttpService httpService, IStocksRepository stocksRepository, IUsersRepository usersRepository)
        {
            _httpService = httpService;
            _stocksRepository = stocksRepository;
            _usersRepository = usersRepository;
        }
        [HttpGet("search")]
        public async Task<IActionResult> GetStockNamesFor(string searched)
        {
            var tickerInfos = await _httpService.GetTickerInfo(searched);
            if (tickerInfos is null)
            {
                return Problem();
            }
            var tickersDto = tickerInfos.results.Select(res => new SearchData
            {
                Symbol = res.ticker,
                Name = res.name,
                Type = res.type,
            }).ToList();
            return Ok(tickersDto);

        }
        [HttpGet("charts/{ticker}")]
        public async Task<IActionResult> GetChartData(string ticker)
        {
            ticker = ticker.ToUpper();
            await Console.Out.WriteLineAsync(ticker);
            var tickerAggregate = await _httpService.GetTickerAggregate(ticker);

            if (tickerAggregate is null)
            {
                var hlocs = await _stocksRepository.GetHlocs(ticker);
                var hlocsDto = hlocs.Select(hl => new ChartDataDto
                {
                    Open = hl.Open,
                    High = hl.High,
                    Low = hl.Low,
                    Close = hl.Close,
                    Volume = hl.Volume,
                    Date = hl.Date,
                }).ToList();
                return Ok(hlocsDto);
            }
            var hlocList = tickerAggregate.results.Select(res => new Hloc
            {
                Open = res.o,
                High = res.h,
                Low = res.l,
                Close = res.c,
                Volume = res.v,
                Date = DateTimeOffset.FromUnixTimeMilliseconds(res.t).DateTime
            }).ToList();
            await _stocksRepository.UpdateHlocks(ticker, hlocList);

            var chartList = tickerAggregate.results.Select(res => new ChartDataDto
            {
                Open = res.o,
                High = res.h,
                Low = res.l,
                Close = res.c,
                Volume = res.v,
                Date = DateTimeOffset.FromUnixTimeMilliseconds(res.t).DateTime
            }).ToList();
            return Ok(chartList);


        }

        [HttpGet("{ticker}/general")]
        public async Task<IActionResult> GetGeneralInformation(string ticker)
        {
            ticker = ticker.ToUpper();
            var info = await _httpService.GetGeneralInfo(ticker);
            if (info is null)
            {
                var stock = await _stocksRepository.GetStock(ticker);
                if (stock is null)
                {
                    return NotFound();
                }
                var dto = new GeneralStockInfoDto
                {
                    Name = stock.Name,
                    Country = stock.Country,
                    ImageUrl = stock.ImageUrl,
                    Ticker = stock.Ticker,
                    Currency = stock.Currency,
                    HomepageUrl = stock.HomepageUrl,
                };
                return Ok(dto);
            }
            var infoDto = new GeneralStockInfoDto
            {
                Name = info.results.name,
                Country = info.results.locale,
                ImageUrl = info.results?.branding?.logo_url,
                HomepageUrl = info.results?.homepage_url,
                Ticker = info.results?.ticker,
                Currency = info.results.currency_name
            };

            if (await _stocksRepository.StockExists(ticker))
            {
                await _stocksRepository.UpdateStock(ticker, info);
            }
            else
            {
                await _stocksRepository.AddStock(new Stock
                {
                    Name = info.results.name,
                    Country = info.results.locale,
                    ImageUrl = info.results.branding.logo_url,
                    HomepageUrl = info.results.homepage_url,
                    Ticker = info.results.ticker,
                    Currency = info.results.currency_name,
                });
            }

            return Ok(infoDto);
        }

        [HttpGet("{ticker}/eos")]
        public async Task<IActionResult> GetEndOfSession(string ticker)
        {
            ticker = ticker.ToUpper();
            var endSesh = await _httpService.GetSessionEndInfo(ticker);
            if (endSesh is null)
            {
                var sesh = await _stocksRepository.GetSessionEnd(ticker);
                if (sesh is null)
                {
                    return NotFound();
                }
                var seshDto = new StockSessionEndDto
                {
                    From = sesh.From,
                    Symbol = ticker,
                    Open = sesh.Open,
                    High = sesh.High,
                    Low = sesh.Low,
                    Close = sesh.Close,
                    Volume = sesh.Volume,
                    AfterHours = sesh.AfterHours,
                    PreMarket = sesh.PreMarket,
                };
                return Ok(seshDto);
            }
            var endSeshDto = new StockSessionEndDto
            {
                From = endSesh.from,
                Symbol = endSesh.symbol,
                Open = endSesh.open,
                High = endSesh.high,
                Low = endSesh.low,
                Close = endSesh.close,
                Volume = endSesh.volume,
                AfterHours = endSesh.afterHours,
                PreMarket = endSesh.preMarket,
            };
            await _stocksRepository.UpdateSessionEnd(ticker, endSeshDto);

            return Ok(endSeshDto);
        }
        [HttpGet("{ticker}/news")]
        public async Task<IActionResult> GetNews(string ticker)
        {
            ticker = ticker.ToUpper();
            var news = await _httpService.GetNews(ticker);
            if (news is null)
            {
                await Console.Out.WriteLineAsync("news is null");
                var newsDb = await _stocksRepository.GetNews(ticker);
                var newsListDto = newsDb.Select(res => new NewsDto
                {
                    PublisherName = res.PublisherName,
                    PublishDate = res.PublishDate,
                    Title = res.Title,
                    AritcleUrl = res.AritcleUrl,
                }).ToList();
                return Ok(newsListDto);
            }
            var newsDto = news.results.Select(res => new NewsDto
            {
                PublisherName = res.publisher.name,
                PublishDate = res.published_utc,
                Title = res.title,
                AritcleUrl = res.article_url
            }).ToList();

            var newData = news.results.Select(res => new News
            {
                PublishDate = res.published_utc,
                PublisherName = res.publisher.name,
                Title = res.title,
                AritcleUrl = res.article_url
            }).ToList();
            await _stocksRepository.UpdateNews(ticker, newData);

            return Ok(newsDto);
        }

        [HttpGet("subs/{username}")]
        public async Task<IActionResult> GetSubscriptions(string username)
        {
            var stocks = await _stocksRepository.GetStocksForUser(username);
            var stocksDto = stocks.Select(stock => new GeneralStockInfoDto
            {
                Name = stock.Name,
                Country = stock.Country,
                ImageUrl = stock.ImageUrl,
                Ticker = stock.Ticker,
                Currency = stock.Currency,
                HomepageUrl = stock.HomepageUrl,

            }).ToList();
            return Ok(stocksDto);
        }

        [HttpPost("subs")]
        public async Task<IActionResult> AddSubscription(StockUserDto stockUserDto)
        {
            stockUserDto.Ticker = stockUserDto.Ticker.ToUpper();
            var user = await _usersRepository.GetUser(stockUserDto.Username);
            if (user == null)
            {
                return NotFound();
            }
            var stock = await _stocksRepository.GetStock(stockUserDto.Ticker);
            if(stock == null)
            {
                return NotFound();
            }
            if (await _stocksRepository.ExistsUserStock(stockUserDto.Ticker, stockUserDto.Username))
            {
                return NoContent();
            }
            var stockUser = new UserStock
            {
                User = user,
                Stock = stock
            };
            await _stocksRepository.AddUserStock(stockUser);
            return Created("", "");
        }


        [HttpDelete("subs/{username}/{ticker}")]
        public async Task<IActionResult> DeleteSubscription(string username, string ticker)
        {
            await _stocksRepository.DeleteUserStockIfExists(username, ticker);
            return NoContent();

        }

    }
}
