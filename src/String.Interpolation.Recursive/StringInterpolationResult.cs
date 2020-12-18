using System;

namespace String.Interpolation.Recursive
{
    internal class StringInterpolationResult
    {
        public StringInterpolationResult(string value, string originalKey) {
            if (string.IsNullOrWhiteSpace(originalKey)) {
                throw new ArgumentException($"'{nameof(originalKey)}' cannot be null or whitespace", nameof(originalKey));
            }

            Value = value;
            OriginalKey = originalKey;
        }

        public string Value { get; }

        public string OriginalKey { get; }
    }
}