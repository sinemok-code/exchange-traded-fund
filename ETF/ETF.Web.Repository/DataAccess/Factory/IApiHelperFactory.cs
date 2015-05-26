using ETF.Web.Common;
using ETF.Web.Repository.Interfaces;

namespace ETF.Web.Repository.DataAccess.Factory
{
    public interface IApiHelperFactory
    {
        IApiHelper CreateUploadFileApiHelper();
    }
}
