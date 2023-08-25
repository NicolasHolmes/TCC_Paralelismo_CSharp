using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{

    public static class Extension
    {
        public static string RemoveAccents(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(letter) != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public static string NoFormatting(this string Codigo)
        {

            if (Codigo == null)
                return "";
            return Codigo.Replace(".", string.Empty)
                .Replace("-", string.Empty)
                .Replace("/", string.Empty)
                .Replace("\\", string.Empty)
                .Replace(" ", string.Empty)
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Replace("\t", string.Empty)
                .Replace("\n", string.Empty);

        }
        public static string NoFormatting2(this string Codigo)
        {

            if (Codigo == null)
                return "";
            return Codigo.Replace(".", string.Empty)
                .Replace("-", string.Empty)
                .Replace("/", string.Empty)
                .Replace("\\", string.Empty)
                .Replace(" ", string.Empty)
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Replace("\t", string.Empty)
                .Replace("\n", string.Empty);

        }
    }
}
