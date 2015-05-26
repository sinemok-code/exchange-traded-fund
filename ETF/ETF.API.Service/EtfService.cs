namespace ETF.API.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ETF.API.Service.Interface;
    using ETF.PublicEntities;

    public class EtfService : IEtfService
    {
        private readonly IEtfContext etfContext;

        public EtfService(IEtfContext etfContext)
        {
            this.etfContext = etfContext;
        }

        public void SaveEtf(List<IndexEtf> indexEtfList)
        {
            this.etfContext.SaveIndexEtfList(indexEtfList);
        }

        public IEnumerable<EtfWeightedIndex> GetEtfWeightedIndices(DateTime? startDate, DateTime? endDate)
        {
            var indexWeightedIndices = new List<EtfWeightedIndex>();

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
                var dates =
                    indexEtf.Stocks.Where(p => p.Date >= startDate && p.Date <= endDate)
                        .OrderBy(p => p.Date)
                        .Select(p => p.Date)
                        .Distinct()
                        .OrderBy(p => p.Date)
                        .ToList();

                var weightedIndices =
                    dates.Select(
                        date => new EtfWeightedIndex { Date = date, IndexName = indexEtf.IndexName })
                        .ToList();

                var etfWeightedIndex = weightedIndices.OrderBy(p => p.Date).FirstOrDefault();

                /*initialize with 100*/
                if (etfWeightedIndex != null)
                {
                    etfWeightedIndex.Value = 100;
                }

                /*calculate weighted values*/
                for (var i = 1; i < weightedIndices.Count(); i++)
                {
                    var oldValues =
                        indexEtf.Stocks.Where(p => p.Date == weightedIndices[i - 1].Date).ToList();
                    var newValues = indexEtf.Stocks.Where(p => p.Date == weightedIndices[i].Date).ToList();
                    CalculateWeight(weightedIndices[i], oldValues, newValues, weightedIndices[i - 1].Value);
                }

                indexWeightedIndices.AddRange(weightedIndices);
            }

            return indexWeightedIndices;
        }

        private static void CalculateWeight(
            EtfWeightedIndex etfWeightedIndex,
            IEnumerable<Stock> oldValues,
            IEnumerable<Stock> newValues,
            double oldWeight)
        {
            var oldSum = oldValues.Sum(value => value.Price * value.ShareNumber);

            var newSum = newValues.Sum(value => value.Price * value.ShareNumber);

            etfWeightedIndex.Value = Math.Round(newSum * oldWeight / oldSum, 3);
        }
    }
}
