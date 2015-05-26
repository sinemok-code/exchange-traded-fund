namespace ETF.PublicAPI.Controllers
{
    using System.Web.Http;

    using ETF.API.Service.Interface;

    public class FileController : ApiController
    {
        private readonly IFileService fileService;
        private readonly IEtfService etfService;

        public FileController(IFileService fileService, IEtfService etfService)
        {
            this.fileService = fileService;
            this.etfService = etfService;
        }

        [HttpPost]
        public bool Upload(byte[] content)
        {
            var indexEtfList = this.fileService.GetIndexEtfList(content);

            if (indexEtfList == null)
            {
                return false;
            }

            this.etfService.SaveEtf(indexEtfList);

            return true;
        }
    }
}
