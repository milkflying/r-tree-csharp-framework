using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Exceptions;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Utility.Exceptions
{

    public class PageSizeTooSmallException : PageException
    {
        protected Int32 _pageSize, _dataSize;

        public virtual Int32 PageSize
        {
            get { return _pageSize; }
            protected set { _pageSize = value; }
        }
        public virtual Int32 DataSize
        {
            get { return _dataSize; }
            protected set { _dataSize = value; }
        }
        public PageSizeTooSmallException(Int32 pageSize, Int32 dataSize)
            : base()
        {
            Initalize(pageSize, dataSize);
        }
        public PageSizeTooSmallException(Int32 pageSize, Int32 dataSize, String message)
            : base(SetMessage(message, pageSize, dataSize))
        {
            Initalize(pageSize, dataSize);
        }
        public PageSizeTooSmallException(Int32 pageSize, Int32 dataSize, String message, Exception innerException)
            : base(SetMessage(message, pageSize, dataSize), innerException)
        {
            Initalize(pageSize, dataSize);
        }
        public PageSizeTooSmallException(Int32 pageSize, Int32 dataSize, SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Initalize(pageSize, dataSize);
        }

        protected virtual void Initalize(Int32 pageSize, Int32 dataSize)
        {
            PageSize = pageSize;
            DataSize = dataSize;
        }
        private static String SetMessage(String message, Int32 pageSize, Int32 dataSize)
        {
            return message + String.Format("{0}The size of the data is {1} bytes, but the page can only fit {2} bytes of data.", Environment.NewLine, dataSize, pageSize);
        }
    }
}
