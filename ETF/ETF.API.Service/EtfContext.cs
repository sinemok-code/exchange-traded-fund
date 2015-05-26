namespace ETF.API.Service
{
    using System.Collections.Generic;

    using ETF.API.Service.Interface;
    using ETF.PublicEntities;

    public class EtfContext : IEtfContext
    {
        private static List<IndexEtf> IndexEtfList { get; set; }

        public void SaveIndexEtfList(List<IndexEtf> indexEtfList)
        {
            IndexEtfList = indexEtfList;
        }

        public List<IndexEtf> GetIndexEtfList()
        {
            return IndexEtfList;
        }
    }
}
