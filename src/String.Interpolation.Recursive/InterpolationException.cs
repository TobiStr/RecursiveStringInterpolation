using System;
using System.Runtime.Serialization;

namespace String.Interpolation.Recursive
{
    [Serializable]
    public class InterpolationException : Exception
    {
        public InterpolationException() {
        }

        public InterpolationException(string message) : base(message) {
        }

        public InterpolationException(string message, Exception innerException) : base(message, innerException) {
        }

        protected InterpolationException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}