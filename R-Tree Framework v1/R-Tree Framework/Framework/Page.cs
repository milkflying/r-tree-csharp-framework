using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public class Page
    {
        protected Address id;
        protected Byte[] data;
        protected Int64 address;

        public Int64 Address
        {
            get { return address; }
            set { address = value; }
        }

        public Address ID
        {
            get { return id; }
            protected set { id = value; }
        }

        public Byte[] Data
        {
            get { return data; }
            set { data = value; }
        }

        public Page(Address id, Int64 address, Byte[] data)
        {
            ID = id;
            Address = address;
            Data = data;
        }


    }
}
