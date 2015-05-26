namespace ETF.Utilities
{
    using System;
    using System.Globalization;

    public static class Extensions
    {
        public static DateTime ToDateTime(this string s)
        {
            DateTime dtr;
            var tryDtr = DateTime.TryParse(s, out dtr);
            return tryDtr ? dtr : new DateTime();
        }

        public static int ToInt(this string number, int defaultInt)
        {
            var resultNum = 0;

            try
            {
                if (!string.IsNullOrEmpty(number))
                {
                    resultNum = Convert.ToInt32(number);
                }
            }
            catch (Exception)
            {
                resultNum = defaultInt;
            }

            return resultNum;
        }

        public static double ToDouble(this string input, bool throwExceptionIfFailed = false)
        {
            double result;
            var valid = double.TryParse(
                input,
                NumberStyles.AllowDecimalPoint,
                new NumberFormatInfo { NumberDecimalSeparator = "." },
                out result);
            if (valid)
            {
                return result;
            }

            if (throwExceptionIfFailed)
            {
                throw new FormatException(string.Format("'{0}' cannot be converted as double", input));
            }

            return result;
        }
    }
}
