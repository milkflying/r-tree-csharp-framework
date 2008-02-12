using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using R_Tree_Framework.Framework;

namespace R_Tree_Framework.Exceptions
{
    public class PerformanceAnalyzerException : RTreeFrameworkException
    {
        public PerformanceAnalyzerException()
            : base()
        {
        }
        public PerformanceAnalyzerException(String message)
            : base(message)
        {
        }
        public PerformanceAnalyzerException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public PerformanceAnalyzerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
