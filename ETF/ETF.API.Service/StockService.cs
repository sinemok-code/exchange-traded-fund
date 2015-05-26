namespace ETF.API.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ETF.API.Service.Interface;
    using ETF.PublicEntities;

    public class StockService : IStockService
    {
        private readonly IEtfContext etfContext;

        public StockService(IEtfContext etfContext)
        {
            this.etfContext = etfContext;
        }

        public IEnumerable<StockWeightedIndex> GetWeightedIndices(DateTime? startDate, DateTime? endDate)
        {
            var weightedIndex = new List<StockWeightedIndex>();

            StockWeightedIndex previousStockWeightedIndex = null;

            if (startDate == null)
            {
                startDate = DateTime.MinValue;
            }

            if (endDate == null)
            {
                endDate = DateTime.MaxValue;
            }

            var indexEtfList = this.etfContext.GetIndexEtfList();
            foreach (var indexEtf in indexEtfList)
            {
                /*get ordered date list*/
                var stocks =
                    indexEtf.Stocks.Where(p => p.Date >= startDate && p.Date <= endDate)
                        .OrderBy(p => p.Date)
                        .GroupBy(p => p.Name)
                        .ToList();

                foreach (var groupedStock in stocks)
                {
                    var count = 0;

                    foreach (var stock in groupedStock)
                    {
                        var weight = count == 0
                                         /*first element*/
                                         ? 100
                                         : CalculateWeight(
                                             previousStockWeightedIndex.Stock,
                                             stock,
                                             previousStockWeightedIndex.Value);
                        var stockWeightedIndex = new StockWeightedIndex { Stock = stock, Value = Math.Round(weight, 3) };

                        weightedIndex.Add(stockWeightedIndex);

                        previousStockWeightedIndex = stockWeightedIndex;
                        count++;
                    }
                }
            }

            return weightedIndex;
        }

        public IEnumerable<StockWeightedIndex> GetTopStockIndices(DateTime? endDate)
        {
            var weightedIndices = this.GetWeightedIndices(null, endDate);

            var lastDate = weightedIndices.Max(x => x.Stock.Date);

            return weightedIndices.Where(x => x.Stock.Date == lastDate).Take(5).OrderByDescending(x => x.Value).ToList();
        }

        public IEnumerable<StockReturn> GetStockReturn(string stockId, DateTime? startDate, DateTime? endDate)
        {
            var stockReturns = new List<StockReturn>();

            Stock previousStock = null;

            if (startDate == null)
            {
                startDate = DateTime.MinValue;
            }

            if (endDate == null)
            {
                endDate = DateTime.MaxValue;
            }

            var indexEtfList = this.etfContext.GetIndexEtfList();
            foreach (var indexEtf in indexEtfList)
            {
                /*get ordered date list*/
                var stocks =
                    indexEtf.Stocks.Where(p => p.Date >= startDate && p.Date <= endDate && p.Id == stockId)
                        .OrderBy(p => p.Date)
                        .GroupBy(p => p.Name)
                        .ToList();

                foreach (var groupedStock in stocks)
                {
                    /*first element*/
                    var count = 0;

                    foreach (var stock in groupedStock)
                    {
                        if (count == 0)
                        {
                            previousStock = stock;
                            count++;
                            continue;
                        }

                        var stockReturn = new StockReturn
                                              {
                                                  Date = stock.Date,
                                                  Value = CalculateWeight(previousStock, stock)
                                              };

                        stockReturns.Add(stockReturn);

                        previousStock = stock;
                        count++;
                    }
                }
            }

            return stockReturns;
        }

        private static double CalculateWeight(Stock oldValue, Stock newValue)
        {
            return (newValue.Price - oldValue.Price) / oldValue.Price;
        }

        private static double CalculateWeight(Stock oldValue, Stock newValue, double oldWeight)
        {
            var oldSum = oldValue.Price * oldValue.ShareNumber;

            var newSum = newValue.Price * newValue.ShareNumber;

            return newSum * oldWeight / oldSum;
        }
    }
}
