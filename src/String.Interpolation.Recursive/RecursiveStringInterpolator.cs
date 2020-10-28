using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace String.Interpolation.Recursive
{
    public class RecursiveStringInterpolator
    {
        private const string REGEX_BASE = @"(?:openingTag)([^closingTag]*)(?:closingTag)";

        private readonly string openingTag;

        private readonly string closingTag;

        private readonly string regex;

        /// <summary>
        /// Initializes a new <see cref="RecursiveStringInterpolator"/> with custom opening and closing tags of the variables.
        /// </summary>
        /// <param name="openingTag"></param>
        /// <param name="closingTag"></param>
        public RecursiveStringInterpolator(string openingTag, string closingTag) {
            if (string.IsNullOrWhiteSpace(openingTag))
                throw new ArgumentException($"Argument cannot be null or whitespace: {nameof(openingTag)}");
            if (string.IsNullOrWhiteSpace(closingTag))
                throw new ArgumentException($"Argument cannot be null or whitespace: {nameof(closingTag)}");

            this.openingTag = openingTag;
            this.closingTag = closingTag;

            regex = REGEX_BASE
                .Replace("openingTag", GetRegexableTag(openingTag))
                .Replace("closingTag", GetRegexableTag(closingTag));
        }

        /// <summary>
        /// Interpolates a given <paramref name="source"/> string with values given in the <paramref name="table"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public string Interpolate(string source, IReadOnlyDictionary<string, string> table)
            => InterpolateRecursive(new StringInterpolationResult(source, source), table, new HashSet<string>()).Value;

        /// <summary>
        /// Recursively traverses each path of the interpolation until it detects a loop, or cannot find any more matches.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="table"></param>
        /// <param name="usedKeys"></param>
        /// <returns></returns>
        private StringInterpolationResult InterpolateRecursive(
            StringInterpolationResult source,
            IReadOnlyDictionary<string, string> table,
            HashSet<string> usedKeys
        ) {
            var matches = Regex.Matches(source.Value, regex);

            var replaceableMatches =
                matches
                    .AsValueEnumerable()
                    .Where(s => table.ContainsKey(s));

            var interpolatedMatches =
                replaceableMatches
                    .Select(s =>
                        {
                            var result = new StringInterpolationResult(table[s], s);
                            if (usedKeys.Contains(result.OriginalKey))
                                throw new InterpolationException(
                                    $"Loop detected: {result.OriginalKey} is interpolated to itself.");
                            return InterpolateRecursive(
                                source: result,
                                table: table,
                                usedKeys: new HashSet<string>(usedKeys.Concat(new [] { result.OriginalKey }))
                            );
                        }
                    );

            return new[] { source }
                .Concat(interpolatedMatches)
                .Aggregate((s1, s2) =>
                    new StringInterpolationResult(
                        value: s1.Value.Replace(
                            oldValue: openingTag + s2.OriginalKey + closingTag,
                            newValue: s2.Value),
                        originalKey: s1.OriginalKey)
                );
        }

        /// <summary>
        /// Prepends all punctuation characters with "\"
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private string GetRegexableTag(string tag) {
            bool IsSymbol(char c) =>
                Regex.Match(c.ToString(), @"[\p{P}]").Success || c == '§' || c == '°' || c == '$';
            return string.Join("", tag.Select(c => IsSymbol(c) ? $@"\{c}" : c.ToString()));
        }
    }
}