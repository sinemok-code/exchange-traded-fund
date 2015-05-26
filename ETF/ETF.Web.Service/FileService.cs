namespace ETF.Web.Service
{
    using ETF.Web.Repository.Interfaces;
    using ETF.Web.Service.Interfaces;
    using ETF.Web.ViewModel;

    public class FileService : IFileService
    {
        private readonly IFileRepository fileRepository;

        public FileService(IFileRepository fileRepository)
        {
            this.fileRepository = fileRepository;
        }

        public UploadFileViewModel CreateUploadFileViewModel()
        {
            return new UploadFileViewModel();
        }

        public void UploadFile(UploadFileViewModel viewModel)
        {
            var uploaded = this.fileRepository.UploadFiles(viewModel.File);

            viewModel.DisplayMessage = true;
            viewModel.Success = uploaded;
        }
    }
}
