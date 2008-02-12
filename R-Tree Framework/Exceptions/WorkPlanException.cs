using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using R_Tree_Framework.Framework;

namespace R_Tree_Framework.Exceptions
{
    public class WorkPlanException : RTreeFrameworkException
    {
        public WorkPlanException()
            : base()
        {
        }
        public WorkPlanException(String message)
            : base(message)
        {
        }
        public WorkPlanException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public WorkPlanException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
