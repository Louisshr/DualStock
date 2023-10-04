using System;
using DualStock.Model.DTO;
using DualStock.Services;

namespace DualStock.DAL
{
    public class StockRepo : IStockRepo
    {
        private readonly StockCacheService _stockCacheService;
        public StockRepo(StockCacheService stockCacheService)
        {
            _stockCacheService = stockCacheService;
        }

        public StockData test()
        {
            //Console.WriteLine("fra repo: ");
            StockData dto = _stockCacheService.getStockDTO();
            return dto;
        }        
    }
}

