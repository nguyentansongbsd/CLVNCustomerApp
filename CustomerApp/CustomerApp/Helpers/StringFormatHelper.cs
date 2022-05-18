using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CustomerApp.Helpers
{
    public class StringFormatHelper
    {
        public static string FormatCurrency(decimal? input)
        {
            if (input.HasValue)
            {
                if (input.Value == 0)
                    return "0";
                else if ("en" == "en")
                    return string.Format("{0:#,##0.##}", input.Value); // luôn có 2 số thập phân 0.00 thay ## nếu k cần
                else
                    return String.Format(new CultureInfo("vi-VN"), "{0:#,##0.##}", input.Value);
            }
            return null;
        }
        public static string FormatPercent(decimal? input)
        {
            if (input.HasValue)
            {
                if (input.Value == 0)
                    return "0";
                else if ("en" == "en")
                    return string.Format("{0:#,##0.00}", input.Value); // luôn có 2 số thập phân 0.00 thay ## nếu k cần
                else
                    return String.Format(new CultureInfo("vi-VN"), "{0:#,##0.00}", input.Value);
            }
            return null;
        }
    }
}
