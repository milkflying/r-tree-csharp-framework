using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public interface Index
    {
        void Insert(Record record);
        void Delete();
        void Update();
        List<Record> Search(Query query);
    }
}