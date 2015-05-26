namespace ETF.PublicAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;

    using ETF.API.Service.Interface;
    using ETF.PublicEntities;

    public class StockController : ApiController
    {
        private readonly IStockService stockService;

        public StockController(IStockService stockService)
        {
            this.stockService = stockService;
        }

        /// <summary>
        /// Gets return values for a specific stock within specified dates
        /// </summary>
        /// <param name="stockId">ID of the stock</param>
        /// <param name="startDate">(Optional) Start date for calculation</param>
        /// <param name="endDate">(Optional) End date for calculation</param>
        /// <returns>Stocks return value</returns>
        [HttpGet]
        public IEnumerable<StockReturn> GetStockReturn(string stockId, DateTime? startDate, DateTime? endDate)
        {
            return this.stockService.GetStockReturn(stockId, startDate, endDate);
        }

        /// <summary>
        /// Calculates stock weighted index within specified dates
        /// </summary>
        /// <param name="startDate">(Optional) Start date for calculation</param>
        /// <param name="endDate">(Optional) End date for calculation</param>
        /// <returns>Stock Weighted Indices</returns>
        [HttpGet]
        public IEnumerable<StockWeightedIndex> GetStockWeightedIndices(DateTime? startDate, DateTime? endDate)
        {
           return this.stockService.GetWeightedIndices(startDate, endDate);
        }

        /// <summary>
        /// Calculates top stock indices on specified date
        /// </summary>
        /// <param name="endDate">(Optional) End date for calculation</param>
        /// <returns>Top stocks weighted indices information</returns>
        [HttpGet]
        public IEnumerable<StockWeightedIndex> GetTopStockIndices(DateTime? endDate)
        {
            return this.stockService.GetTopStockIndices(endDate);
        }
    }
}
