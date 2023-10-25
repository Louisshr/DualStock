using System;
using DualStock.DAL;
using DualStock.Model;
using DualStock.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using YahooFinanceApi;

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
        public StockData test()
        {
            return _db.test();
        }

        [HttpGet]
        public async Task<ActionResult<HistoricalStockData>> GetHistoricalDataForSpecificStock()
        {
            try
            {
                var stock = "AAPL";
                var historicalData = await _db.GetHistoricalDataForSpecificStock(stock);
                if (historicalData == null)
                {
                    Console.WriteLine("CONTROLLER: FEIL");
                    return BadRequest($"failed to fetch stock data for the stock {stock}");
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
            var historicalData = await Yahoo.GetHistoricalAsync("AAPL", new DateTime(2023,1,1), new DateTime(2023,3,1), Period.Monthly);
            foreach (var h in historicalData)
            {
                Console.WriteLine("DATE: " + h.DateTime + ", price: " + h.Open);
            }
            return "OK";
        }        
    }    
}

