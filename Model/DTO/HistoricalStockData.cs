using System;
using YahooFinanceApi;

namespace DualStock.Model.DTO
{
    public class HistoricalStockData
    {
        public string symbol { get; set; }
        public string longName { get; set; }
        public string value { get; set; }
        public string change { get; set; }
        public string changePercent { get; set; }
        public string bid { get; set; }
        public string ask { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public long volume { get; set; }
        public IEnumerable<Candle> daily { get; set; }
        public IEnumerable<dynamic> weekly { get; set; }
    }
}

