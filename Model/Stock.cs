using System;
namespace DualStock.Model
{
    public class Stock
    {
        public string symbol { get; set; }
        public string? longName { get; set; }
        public string value { get; set; }
        public string change { get; set; }
        public string changePercent { get; set; }
    }
}

