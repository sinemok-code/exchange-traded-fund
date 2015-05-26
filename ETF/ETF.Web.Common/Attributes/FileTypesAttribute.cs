namespace ETF.Web.Common.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class FileTypesAttribute : ValidationAttribute
    {
        private readonly List<string> types;

        public FileTypesAttribute(string typesAttr)
        {
            this.types = typesAttr.Split(',').ToList();
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            var httpPostedFile = value as HttpPostedFileWrapper;

            if (httpPostedFile == null)
            {
                return false;
            }

            var extension = System.IO.Path.GetExtension(httpPostedFile.FileName);

            if (extension == null)
            {
                return false;
            }

            var fileExt = extension.Substring(1);
            return this.types.Contains(fileExt, StringComparer.OrdinalIgnoreCase);
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(
                "Invalid file type. Only the following types {0} are supported.",
                string.Join(", ", this.types));
        }
    }
}