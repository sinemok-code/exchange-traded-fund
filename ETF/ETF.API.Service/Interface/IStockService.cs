namespace ETF.API.Service.Interface
{
    using System;
    using System.Collections.Generic;

    using ETF.PublicEntities;

    public interface IStockService
    {
        IEnumerable<StockWeightedIndex> GetWeightedIndices(DateTime? startDate, DateTime? endDate);

        IEnumerable<StockWeightedIndex> GetTopStockIndices(DateTime? endDate);

        IEnumerable<StockReturn> GetStockReturn(string stockId, DateTime? startDate, DateTime? endDate);
    }
}
