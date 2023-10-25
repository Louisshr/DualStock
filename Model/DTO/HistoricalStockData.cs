using System;
using YahooFinanceApi;

namespace DualStock.Model.DTO
{
    public class HistoricalStockData
    {
        public IEnumerable<Candle> daily { get; set; }
        public IEnumerable<dynamic> weekly { get; set; }
    }
}

