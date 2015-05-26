namespace ETF.Web.Common.Exceptions
{
    using System;

    public class AppException : Exception
    {
        private const string DefaultMessage = "An application exception has occurred.";

        public AppException(
            Reference.StatusCode statusCode = Reference.StatusCode.Error,
            string message = DefaultMessage)
            : base(SetMessage(statusCode, message))
        {
            this.StatusCode = statusCode;
        }

        public Reference.StatusCode StatusCode { get; private set; }

        private static string SetMessage(Reference.StatusCode statusCode, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }

            switch (statusCode)
            {
                case Reference.StatusCode.NotFound:
                    return Reference.Dictionary.PageNotFoundError;

                default:
                    return Reference.Dictionary.InternalError;
            }
        }
    }
}