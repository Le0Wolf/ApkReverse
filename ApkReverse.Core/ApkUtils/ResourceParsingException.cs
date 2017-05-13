using System;

namespace ApkReverse.Core.ApkUtils.ResourcesModels
{
    using System.Runtime.Serialization;
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ResourceParsingException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ResourceParsingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {}

        public ResourceParsingException(string message)
            : base(message)
        {}
    }
}
