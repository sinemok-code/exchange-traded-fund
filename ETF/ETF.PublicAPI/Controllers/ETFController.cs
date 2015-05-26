namespace ETF.PublicAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;

    using ETF.API.Service.Interface;
    using ETF.PublicEntities;

    public class ETFController : ApiController
    {
        private readonly IEtfService etfService;

        public ETFController(IEtfService etfService)
        {
            this.etfService = etfService;
        }

        /// <summary>
        /// Calculates ETF weighted index within specified dates
        /// </summary>
        /// <param name="startDate">(Optional) Start date for calculation</param>
        /// <param name="endDate">(Optional) End date for calculation</param>
        /// <returns>ETF Weighted Indices</returns>
        [HttpGet]
        public IEnumerable<EtfWeightedIndex> GetEtfWeightedIndices(DateTime? startDate, DateTime? endDate)
        {
            return this.etfService.GetEtfWeightedIndices(startDate, endDate);
        }
    }
}