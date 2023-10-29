using System;
using System.Threading.Channels;
using DualStock.Model;
using DualStock.Model.DTO;
using OoplesFinance.YahooFinanceAPI;
using OoplesFinance.YahooFinanceAPI.Enums;
using OoplesFinance.YahooFinanceAPI.Models;
using YahooFinanceApi;

namespace DualStock.Services
{
    public class StockCacheService
    {
        private readonly StockData stockDTO = new StockData();
        private readonly Timer _timer;
        private bool isUpdating = false;
        
        public StockCacheService()
        {
            Console.WriteLine("STARTING: stockCacheService");                        
            _timer = new Timer(async _ => await timerCallback(), null, 0, 300000);                                                                
            //Timer timer = new Timer(async _ => await updateStockCache(), null, 0, 60000);            
            //initializeTest();
            //updateStockCache().GetAwaiter().GetResult();            
        }

        private async Task timerCallback()
        {
            if (!this.isUpdating)
            {                
                this.isUpdating = true;
                //Console.WriteLine("STATUS: HENTER AKSJE DATA");
                //await updateStockCache();
                await helloWorld();
            }
        }

        public async Task updateStockCache()
        {
            try
            {
                var yahooClient = new YahooClient();
                var topTrendingList = await yahooClient.GetTopTrendingStocksAsync(Country.UnitedStates, 10);                
                var securities = await Yahoo.Symbols(topTrendingList.ToArray()).Fields(Field.Symbol, Field.LongName, Field.RegularMarketPrice, Field.RegularMarketChange, Field.RegularMarketChangePercent).QueryAsync();
                List<Stock> trendingStocks = new List<Stock>();
                
                foreach (var security in securities)
                {
                    try
                    {   
                        var stock = securities[security.Key];
                        string? currentLongName = null;
                        // Checking if the field, longName, exists. If not, the current stock will still be made a stock object of,
                        // but it will not have the correct value in the longName-field, but rather null. 
                        try { currentLongName = stock[Field.LongName]; }
                        catch (Exception e) { Console.WriteLine(e); }

                        Stock aStock = new Stock()
                        {
                            symbol = stock[Field.Symbol],
                            longName = currentLongName,
                            value = string.Format("{0:0.00}", stock[Field.RegularMarketPrice]),
                            change = string.Format("{0:0.00}", stock[Field.RegularMarketChange]),
                            changePercent = string.Format("{0:0.00}", stock[Field.RegularMarketChangePercent])
                        };
                        trendingStocks.Add(aStock);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("FEIL");
                        Console.WriteLine(e);
                    }
                }                               
                this.stockDTO.trendingStocks = trendingStocks;
                
                // RETRIEVING STOCKGAINERS            
                var gainersList = await yahooClient.GetTopGainersAsync(10);
                var losersList = await yahooClient.GetTopLosersAsync(10);
                List<ScreenerResult?> screenerResults = new List<ScreenerResult?>();
                screenerResults.Add(gainersList);
                screenerResults.Add(losersList);
                bool isGainer = true;

                foreach (var screenerResult in screenerResults)
                {
                    if (screenerResult == null || screenerResult.Quotes == null) { continue; }
                    List<Stock> screener = new List<Stock>();
                    foreach (var security in screenerResult.Quotes)
                    {
                        try
                        {
                            Stock aStock = new Stock()
                            {
                                symbol = security.Symbol,
                                longName = security.LongName,
                                value = string.Format("{0:0.00}", security.RegularMarketPrice),
                                change = string.Format("{0:0.00}", security.RegularMarketChange),
                                changePercent = string.Format("{0:0.00}", security.RegularMarketChangePercent)
                            };
                            screener.Add(aStock);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                    if (isGainer)
                    {
                        this.stockDTO.stockGainers = screener;
                        isGainer = false;
                    }
                    else { this.stockDTO.stockLosers = screener; }
                }

                /*
                foreach (var top in trendingStocks) { Console.WriteLine("SYMBOL: " + top.symbol + ", VALUE: " + top.value + ", CHANGE: " + top.change + ", CHANGE P: " + top.changePercent); }
                Console.WriteLine("g");
                foreach (var top in this.stockDTO.stockGainers) { Console.WriteLine("SYMBOL: " + top.symbol + ", LONGNAME: " + top.longName + ", VALUE: " + top.value + ", CHANGE: " + top.change + ", CHANGE P: " + top.changePercent); }
                Console.WriteLine("l");
                foreach (var top in this.stockDTO.stockLosers) { Console.WriteLine("SYMBOL: " + top.symbol + ", LONGNAME: " + top.longName + ", VALUE: " + top.value + ", CHANGE: " + top.change + ", CHANGE P: " + top.changePercent); }
                */
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                this.isUpdating = false;
            }
        }

        public StockData getStockDTO()
        {
            return this.stockDTO;
        }
        
        public void initializeTest()
        {            
            Stock stock1 = new Stock()
            {
                symbol = "SE",
                longName = "Sea Limited",
                value = "40,20",
                change = "4,24",
                changePercent = "11,79"                
            };
            Stock stock2 = new Stock()
            {
                symbol = "RNAZ",
                longName = "TransCode Therapeutics, Inc.",
                value = "2,55",
                change = "1,89",
                changePercent = "284,04"                
            };
            List<Stock> trendingStocks = new List<Stock>();
            trendingStocks.Add(stock1);
            trendingStocks.Add(stock2);
            this.stockDTO.trendingStocks = trendingStocks;            
        }        

        public static async void test()
        {
            string[] topTrendingList2 = new string[] { "AAPL" };
            var securities = await Yahoo.Symbols(topTrendingList2).Fields(Field.Symbol, Field.LongName, Field.RegularMarketPrice, Field.RegularMarketChange, Field.RegularMarketChangePercent).QueryAsync();

            foreach (var sec in securities)
            {
                var stock = securities[sec.Key];
                               
                string value = string.Format("{0:0.00}", stock[Field.RegularMarketPrice]);                
                string change = string.Format("{0:0.00}", stock[Field.RegularMarketChange]);                
                string changePercent = string.Format("{0:0.00}", stock[Field.RegularMarketChangePercent]);
                                
                Console.WriteLine("Symbol: " + stock[Field.Symbol] + ", Name: " + stock[Field.LongName] + ", Value: " + value + ", Change: " + change + ", change (%): " + changePercent);
            }
        }

        public async Task helloWorld()
        {
            try
            {
                Console.WriteLine("hello world 2");
            }
            finally
            {                
                this.isUpdating = false;
            }
        }
    }
}