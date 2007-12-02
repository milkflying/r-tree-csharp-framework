using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public interface Index
    {
        void Insert(Record record);
        void Delete(Record record);
        void Update(Record originalRecord, Record newRecord);
        List<Record> Search(Query query); 
    }
}