namespace ETF.API.Service.Interface
{
    using System;
    using System.Collections.Generic;

    using ETF.PublicEntities;

    public interface IEtfService
    {
        void SaveEtf(List<IndexEtf> indexEtfList);

        IEnumerable<EtfWeightedIndex> GetEtfWeightedIndices(DateTime? startDate, DateTime? endDate);


    }
}
