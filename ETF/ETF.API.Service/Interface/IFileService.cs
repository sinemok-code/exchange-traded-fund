namespace ETF.API.Service.Interface
{
    using System.Collections.Generic;

    using ETF.PublicEntities;

    public interface IFileService
    {
        List<IndexEtf> GetIndexEtfList(byte[] content);
    }
}
