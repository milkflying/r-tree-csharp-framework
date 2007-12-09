using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace Edu.Psu.Cse.R_Tree_Framework.Indexes
{
    public class ReservationBuffer
    {
        protected List<BufferItem> buffer;
        protected Int32 bufferSize;

        protected virtual Int32 BufferSize
        {
            get { return bufferSize; }
            set { bufferSize = value; }
        }
        public virtual Boolean NeedFlush
        {
            get { return Buffer.Count > BufferSize; }
        }

        public virtual List<BufferItem> Buffer
        {
            get { return buffer; }
            protected set { buffer = value; }
        }

        public ReservationBuffer(Int32 bufferSize)
        {
            BufferSize = bufferSize;
            Buffer = new List<BufferItem>(BufferSize + 1);
        }

        public void InsertEntry(NodeEntry entry)
        {
            Buffer.Add(new BufferItem(entry, Operation.Insert));
        }
        public void DeleteEntry(NodeEntry entry)
        {
            Buffer.Add(new BufferItem(entry, Operation.Delete));
        }
        public void Clear()
        {
            Buffer.Clear();
        }
    }
}
