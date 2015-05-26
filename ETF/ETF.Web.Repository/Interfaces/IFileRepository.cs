namespace ETF.Web.Repository.Interfaces
{
    using System.Web;

    public interface IFileRepository
    {
        bool UploadFiles(HttpPostedFileBase file);
    }
}
