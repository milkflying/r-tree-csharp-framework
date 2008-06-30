using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Framework;

namespace R_Tree_Framework.Utility
{
    public class Page : UtilityObject, IAddressable, ISavable
    {
        protected Int32 _pageSize;
        protected Byte[] _data;

        public virtual Int32 PageSize
        {
            get { return _pageSize; }
            protected set { _pageSize = value; }
        }
        protected virtual Byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public Page(Int32 pageSize)
        {
            PageSize = pageSize;
            Data = new Byte[PageSize];
        }

        
        public virtual Byte[] GetData(Int32 index, Int32 size)
        {
        }
    }
}
