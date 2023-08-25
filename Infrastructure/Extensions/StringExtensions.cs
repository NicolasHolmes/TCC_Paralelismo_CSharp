using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static List<string> ToCommands(this string text)
        {
            return text.ToUpper().Split(' ').ToList();
        }

        public static string FormatToSave(this string text)
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            return text;
        }
    }
}
