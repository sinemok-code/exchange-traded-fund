namespace ETF.API.Service.Interface
{
    using System.Collections.Generic;

    using ETF.PublicEntities;

    public interface IEtfContext
    {
        void SaveIndexEtfList(List<IndexEtf> indexEtfList);

        List<IndexEtf> GetIndexEtfList();
    }
}
