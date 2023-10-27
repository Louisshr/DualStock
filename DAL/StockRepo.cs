using System;
using DualStock.Model.DTO;
using DualStock.Services;
using YahooFinanceApi;

namespace DualStock.DAL
{
    public class StockRepo : IStockRepo
    {
        private readonly StockCacheService _stockCacheService;
        public StockRepo(StockCacheService stockCacheService)
        {
            _stockCacheService = stockCacheService;
        }

        public StockData? GetTrendingStocksAndGainersLosers()
        {
            try
            {               
                StockData dto = _stockCacheService.getStockDTO();
                return dto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<HistoricalStockData?> GetHistoricalDataForSpecificStock(string ticker)
        {
            try
            {                
                // Retrieving stock data last five business days
                DateTime today = DateTime.Today;
                DateTime twoMonthsBack = today.AddMonths(-2);
                var historicalData = await Yahoo.GetHistoricalAsync(ticker, twoMonthsBack, today, Period.Daily);
                var stockDataLastWeek = historicalData.TakeLast(5);

                // Retrieving stock data last 28 days
                DateTime yesterday = DateTime.Today;
                DayOfWeek previousDay = yesterday.DayOfWeek;

                // offset is the number of days to subtract from current day to get to monday
                int offset = (int)previousDay - (int)DayOfWeek.Monday;
                DateTime lastMonday = yesterday.AddDays(-offset);
                int weeks = 4;
                DateTime currentMonday = lastMonday.AddDays(-7 * weeks);
                List<string> dates = new List<string>();
                do
                {
                    string dateFormatted = currentMonday.ToString("dd.MM.yyyy 00:00:00");
                    dates.Add(dateFormatted);
                    currentMonday = currentMonday.AddDays(7);
                }
                while (currentMonday <= lastMonday);
                var stockDataLastFourMondays = from date in dates
                                               join hd in historicalData
                                               on date equals hd.DateTime.ToString("dd.MM.yyyy 00:00:00") into dhd
                                               from dhdx in dhd.DefaultIfEmpty()
                                               select new
                                               {
                                                   data = dhdx
                                               };

                HistoricalStockData HistoricalStockDataDTO = new HistoricalStockData()
                {
                    daily = stockDataLastWeek,
                    weekly = stockDataLastFourMondays
                };

                return HistoricalStockDataDTO;
            }
            catch (Exception e)
            {
                Console.WriteLine("REPO: FEIL");
                Console.WriteLine(e);
                return null;
            }
            /*
            foreach (var candle in historicalData)
            {                
                Console.WriteLine($"DateTime: {candle.DateTime}, Open: {candle.Open}, High: {candle.High}, Low: {candle.Low}, Close: {candle.Close}, Volume: {candle.Volume}, AdjustedClose: {candle.AdjustedClose}");
            }       */
        }        
    }
}