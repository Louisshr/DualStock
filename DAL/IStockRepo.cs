using System;
using DualStock.Model.DTO;
using YahooFinanceApi;

namespace DualStock.DAL
{
    public interface IStockRepo
    {
        StockData test();
        Task<HistoricalStockData?> GetHistoricalDataForSpecificStock(string stockSymbol);        
    }
}

