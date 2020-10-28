using System;
using System.Collections.Generic;

namespace String.Interpolation.Recursive
{
    internal class StringInterpolationResult
    {
        public StringInterpolationResult(string value, string originalKey) {
            if (string.IsNullOrWhiteSpace(value)) {
                throw new ArgumentException($"'{nameof(value)}' cannot be null or whitespace", nameof(value));
            }

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