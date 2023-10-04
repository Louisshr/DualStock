using System;
namespace DualStock.Model.DTO
{
    public class StockData
    {
        public List<Stock> trendingStocks { get; set; }
        public List<Stock> stockGainers { get; set; }
        public List<Stock> stockLosers { get; set; }
    }
}

