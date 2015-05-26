namespace ETF.Web.Controllers
{
    using System.Web.Mvc;

    using ETF.Web.Service.Interfaces;
    using ETF.Web.ViewModel;

    /// <summary>
    /// Controller for File Upload page
    /// </summary>
    public class FileController : Controller
    {
        private readonly IFileService fileService;

        public FileController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        [HttpGet]
        public ActionResult Upload()
        {
            var viewModel = this.fileService.CreateUploadFileViewModel();

            return this.View("_Upload", viewModel);
        }

        [HttpPost]
        public ActionResult Upload(UploadFileViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                this.fileService.UploadFile(viewModel);
            }
            else
            {
                viewModel.DisplayMessage = true;
            }

            return this.View("_Upload", viewModel);
        }
    }
}