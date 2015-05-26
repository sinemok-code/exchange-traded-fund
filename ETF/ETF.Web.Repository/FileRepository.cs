using System;
using System.Web;
using ETF.Web.Repository.DataAccess.Factory;
using ETF.Web.Repository.Interfaces;

namespace ETF.Web.Repository
{
    using System.IO;

    public class FileRepository : IFileRepository
    {
        private readonly IApiHelperFactory apiHelperFactory;

        public FileRepository(IApiHelperFactory apiHelperFactory)
        {
            this.apiHelperFactory = apiHelperFactory;
        }

        public bool UploadFiles(HttpPostedFileBase file)
        {
            var apiHelper = this.apiHelperFactory.CreateUploadFileApiHelper();

            var target = new MemoryStream();
            file.InputStream.CopyTo(target);
            
            var data = target.ToArray();

            try
            {
                apiHelper.GetResponse<bool>(content: data);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
