using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text;
namespace AvaRaspberry.Serivices
{
    public static class Utils
    {

        public static bool IsNullOrEmpty(this IEnumerable current)
        {
            return current is null || !current.GetEnumerator().MoveNext();
        }

        public static string GetSizeString(long length, bool showEmpty = false, bool isSpeed = false)
        {
            long B = 0, KB = 1024, MB = KB * 1024, GB = MB * 1024, TB = GB * 1024;
            double size = length;
            var suffix = nameof(B);

            double SelSize = 0;

            if (length >= TB)
            {
                SelSize = TB;
                suffix = nameof(TB);
            }
            else if (length >= GB)
            {
                SelSize = GB;
                suffix = nameof(GB);
            }
            else if (length >= MB)
            {
                SelSize = MB;
                suffix = nameof(MB);
            }
            else if (length >= KB)
            {
                SelSize = KB;
                suffix = nameof(KB);
            }
            else if (length >= B)
            {
                return $"{size} {nameof(B)}";
            }
            else
            {
                var data =  showEmpty ? $"0 {nameof(KB)}" : string.Empty;

                if(!string.IsNullOrEmpty(data) && isSpeed)
                {
                    return $"{data}/s";
                }
                return data;
            }

            size = Math.Round(length / SelSize, 2);

            var lngt = $"{size} {suffix}";

            if (!string.IsNullOrEmpty(lngt) && isSpeed)
            {
                return $"{lngt}/s";
            }
            return lngt;
        }
    }
}
