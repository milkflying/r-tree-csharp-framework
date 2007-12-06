using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public class Address : IComparable, IComparable<Address>, IEquatable<Address>
    {
        protected static Int32 nextAddress = 1;
        protected Int32 address;
        
        protected static Int32 NextAddress
        {
            get { return nextAddress; }
            set { nextAddress = value; }
        }
        protected Int32 AddressValue
        {
            get { return address; }
            set { address = value; }
        }
        public static Address Empty
        {
            get { return new Address(0); }
        }
        public static Address NewAddress()
        {
            return new Address();
        }
        public override Boolean Equals(object obj)
        {
            if (obj is Address)
                return this.AddressValue.Equals((obj as Address).AddressValue);
            else
                throw new Exception();
        }
        public Address()
        {
            AddressValue = NextAddress++;
        }
        protected Address(Int32 address)
        {
            AddressValue = address;
        }
        public override String ToString()
        {
            return AddressValue.ToString();
        }
        public Address(String address)
        {
            AddressValue = Int32.Parse(address);
        }
        public Address(Byte[] bytes)
        {
            AddressValue = BitConverter.ToInt32(bytes, 0);
        }
        public virtual Byte[] ToByteArray()
        {
            return BitConverter.GetBytes(AddressValue);
        }
        public Int32 CompareTo(Object obj)
        {
            if (obj is Address)
                return AddressValue.CompareTo((obj as Address).AddressValue);
            else
                throw new Exception();
        }
        public Int32 CompareTo(Address obj)
        {
            return AddressValue.CompareTo((obj as Address).AddressValue);
        }
        public Boolean Equals(Address obj)
        {
            return this.AddressValue.Equals(obj.AddressValue);
        }
        public static Boolean operator ==(Address a, Address b)
        {
            if (System.Object.ReferenceEquals(a, b))
                return true;
            if (((object)a == null) || ((object)b == null))
                return false;
            return a.AddressValue == b.AddressValue;

        }
        public static Boolean operator !=(Address a, Address b)
        {
            return !(a == b);

        }

    }
}
