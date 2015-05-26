using ETF.Web.Common;
using ETF.Web.Repository.Interfaces;

namespace ETF.Web.Repository.DataAccess.Factory
{
    public class ApiHelperFactory : IApiHelperFactory
    {
        public IApiHelper CreateUploadFileApiHelper()
        {
            var apiHelper =
                IoC.Resolve<IApiHelper>(Dictionary.UploadFileApiData);

            return apiHelper;
        }
    }
}
