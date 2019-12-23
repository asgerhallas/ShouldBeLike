using System;
using System.Runtime.Serialization;

namespace ShouldBeLike
{
    [Serializable]
    public class ShouldBeLikeException : Exception
    {
        public ShouldBeLikeException() { }
        public ShouldBeLikeException(string message) : base(message) { }
        public ShouldBeLikeException(string message, Exception inner) : base(message, inner) { }

        protected ShouldBeLikeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}