namespace ETF.Web.Common.Attributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly int maxSize;

        public FileSizeAttribute(int maxSizeAttr)
        {
            this.maxSize = maxSizeAttr;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            var httpPostedFile = value as HttpPostedFileWrapper;
            return httpPostedFile != null && this.maxSize > httpPostedFile.ContentLength;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("The file size should not exceed {0}", this.maxSize);
        }
    }
}