using System;
using DualStock.DAL;
using DualStock.Model;
using DualStock.Model.DTO;
using Microsoft.AspNetCore.Mvc;

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
       
    }    
}

