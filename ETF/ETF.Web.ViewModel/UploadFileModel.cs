namespace ETF.Web.ViewModel
{
    using System.Web;

    using ETF.Web.Common.Attributes;

    public class UploadFileViewModel
    {
        [FileSize(40960)]
        [FileTypes("csv")]
        public HttpPostedFileBase File { get; set; }

        public bool DisplayMessage { get; set; }

        public bool Success { get; set; }
    }
}
