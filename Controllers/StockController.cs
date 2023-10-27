using System;
using DualStock.DAL;
using DualStock.Model;
using DualStock.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using YahooFinanceApi;
using System.Text.RegularExpressions;

namespace DualStock.Controllers
{
    [Route("[controller]/[action]")]
    public class StockController : ControllerBase
    {
        private readonly IStockRepo _db;
        public StockController(IStockRepo db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<StockData> GetTrendingStocksAndGainersLosers()
        {
            try
            {
                var data = _db.GetTrendingStocksAndGainersLosers();
                if (data == null)
                {
                    return BadRequest("Error when fetching stocktypes: trending, gainer and loser");
                }
                return Ok(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Error when fetching stocktypes: trending, gainer and loser");
            }
        }

        [HttpGet("{ticker}")]
        public async Task<ActionResult<HistoricalStockData>> GetHistoricalDataForSpecificStock(string ticker)
        {
            try
            {
                Regex tickerRegex = new Regex(@"^\S*^[A-Z]*$");
                if (!tickerRegex.IsMatch(ticker))
                {
                    Console.WriteLine("REGEX failed");
                    return BadRequest("Regular expression test failed");
                }
                var historicalData = await _db.GetHistoricalDataForSpecificStock(ticker);
                if (historicalData == null)
                {
                    Console.WriteLine("FROM CONTROLLER: Error in controller for method GetHistoricalDataForSpecificStock");
                    return BadRequest($"failed to fetch stock data for the stock ticker: {ticker}");
                }
                return Ok(historicalData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("error occured when fetching stock data");
            }
        }

        [HttpGet]
        public string helloWorld()
        {
            return "hello world";
        }

        [HttpGet]
        public async Task<string> dateTest2()
        {
            var historicalData = await Yahoo.GetHistoricalAsync("AAPL", new DateTime(2023, 1, 1), new DateTime(2023, 3, 1), Period.Monthly);
            foreach (var h in historicalData)
            {
                Console.WriteLine("DATE: " + h.DateTime + ", price: " + h.Open);
            }
            return "OK";
        }                
    }    
}

