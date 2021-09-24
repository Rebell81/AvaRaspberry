using System;
using System.Collections;

namespace AvaRaspberry.Extenstion
{
    public static class Utils
    {

        public static bool IsNullOrEmpty(this IEnumerable current)
        {
            return current is null || !current.GetEnumerator().MoveNext();
        }

        public static bool IsNullOrEmpty(this string current)
        {
            return string.IsNullOrEmpty(current);
        }
    }
}
