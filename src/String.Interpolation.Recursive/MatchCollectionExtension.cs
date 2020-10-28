using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace String.Interpolation.Recursive
{
    internal static class MatchCollectionExtension
    {
        public static IEnumerable<string> AsValueEnumerable(this MatchCollection matchCollection) {
            var enumerator = matchCollection.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext()) {
                yield return ((Match)enumerator.Current).Groups[1].Value;
            }
        }
    }
}