using System;
using DualStock.Model.DTO;
using YahooFinanceApi;

namespace DualStock.DAL
{
    public interface IStockRepo
    {
        StockData? GetTrendingStocksAndGainersLosers();
        Task<HistoricalStockData?> GetHistoricalDataForSpecificStock(string ticker);        
    }
}

