namespace ETF.Web.Service.Interfaces
{
    using ETF.Web.ViewModel;

    public interface IFileService
    {
        UploadFileViewModel CreateUploadFileViewModel();

        void UploadFile(UploadFileViewModel viewModel);
    }
}
