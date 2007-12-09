using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public class Sector : PageData
    {
        protected Address address;
        protected List<IndexUnit> indexUnits;

        public virtual List<IndexUnit> IndexUnits
        {
            get { return indexUnits; }
            protected set { indexUnits = value; }
        }
        public virtual Address Address
        {
            get { return address; }
            protected set { address = value; }
        }
        
        public Sector(int maxIndexUnits)
        {
            Address = Address.NewAddress();
            IndexUnits = new List<IndexUnit>(maxIndexUnits + 1);
        }
        public Sector(Address address, Byte[] data)
        {
            Address = address;
            Int32 index = 1, indexUnitCount;

            indexUnitCount = BitConverter.ToInt16(data, index);
            index += 2;

            IndexUnits = new List<IndexUnit>(indexUnitCount + 1);
            for (int i = 0; i < indexUnitCount; i++)
            {
                Byte[] indexUnitData = new Byte[Constants.INDEX_UNIT_SIZE];
                Array.Copy(data, index, indexUnitData, 0, Constants.INDEX_UNIT_SIZE);
                AddIndexUnit(new IndexUnit(indexUnitData));
                index += Constants.INDEX_UNIT_SIZE;
            }
        }

        public virtual Byte[] GeneratePageData()
        {
            Int32 index = 0;
            
            Byte[] data = new Byte[Constants.SECTOR_SIZE];
            
            data[index++] = (Byte)PageDataType.IndexUnitSector;
            
            BitConverter.GetBytes((Int16)IndexUnits.Count).CopyTo(data, index);
            index += 2;

            foreach (IndexUnit indexUnit in IndexUnits)
            {
                indexUnit.GetBytes().CopyTo(data, index);
                index += Constants.INDEX_UNIT_SIZE;
            }
            return data;
        }
        public virtual void AddIndexUnit(IndexUnit indexUnit)
        {
            IndexUnits.Add(indexUnit);
        }
        public virtual void RemoveIndexUnit(IndexUnit indexUnit)
        {
            IndexUnits.Remove(indexUnit);
        }
    }
}
