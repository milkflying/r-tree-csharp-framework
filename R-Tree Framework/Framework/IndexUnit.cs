using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{

    public class IndexUnit
    {
        #region Instance Variables

        protected Address node, child;
        protected MinimumBoundingBox childBoundingBox;
        protected Operation operation;

        #endregion
        #region Properties

        public virtual Operation Operation
        {
            get { return operation; }
            protected set { operation = value; }
        }
        public virtual Address Child
        {
            get { return child; }
            protected set { child = value; }
        }
        public virtual Address Node
        {
            get { return node; }
            protected set { node = value; }
        }
        public virtual MinimumBoundingBox ChildBoundingBox
        {
            get { return childBoundingBox; }
            protected set { childBoundingBox = value; }
        }

        #endregion
        #region Constructors

        public IndexUnit(Address node, Address child, MinimumBoundingBox childBoundingBox, Operation operation)
        {
            Operation = operation;
            Node = node;
            Child = child;
            ChildBoundingBox = childBoundingBox;
        }
        public IndexUnit(Byte[] data)
        {
            if (data[0] == (Byte)0)
                Operation = Operation.Insert;
            else if (data[0] == (Byte)1)
                Operation = Operation.Delete;
            else
                Operation = Operation.Update;
            Int32 index = 1;
            
            Byte[] nodeAddress = new Byte[Constants.ADDRESS_SIZE], childAddress = new Byte[Constants.ADDRESS_SIZE];

            Array.Copy(data, index, nodeAddress, 0, Constants.ADDRESS_SIZE);
            index += Constants.ADDRESS_SIZE;
            Node = new Address(nodeAddress);

            Array.Copy(data, index, childAddress, 0, Constants.ADDRESS_SIZE);
            index += Constants.ADDRESS_SIZE;
            Child = new Address(childAddress);

            ChildBoundingBox = new MinimumBoundingBox(
                BitConverter.ToSingle(data, index),
                BitConverter.ToSingle(data, index +4),
                BitConverter.ToSingle(data, index+8),
                BitConverter.ToSingle(data, index+12));
        }

        #endregion
        #region Public Methods

        public virtual Byte[] GetBytes()
        {
            Byte[] data = new Byte[Constants.INDEX_UNIT_SIZE];
            Int32 index = 0;
            data[index++] = (Byte)Operation;

            Node.ToByteArray().CopyTo(data, index);
            index += Constants.ADDRESS_SIZE;

            Child.ToByteArray().CopyTo(data, index);
            index += Constants.ADDRESS_SIZE;

            BitConverter.GetBytes(ChildBoundingBox.MinX).CopyTo(data, index);
            index += 4;
            BitConverter.GetBytes(ChildBoundingBox.MinY).CopyTo(data, index);
            index += 4;
            BitConverter.GetBytes(ChildBoundingBox.MaxX).CopyTo(data, index);
            index += 4;
            BitConverter.GetBytes(ChildBoundingBox.MaxY).CopyTo(data, index);
            return data;
        }

        #endregion
    }
}
