using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Framework;

namespace R_Tree_Framework.Utility
{
    public class Pair<T1, T2> : UtilityObject
    {
        private T1 value1;
        private T2 value2;

        public Pair(T1 value1, T2 value2)
        {
            this.value1 = value1;
            this.value2 = value2;
        }

        public T1 Value1
        {
            get { return value1; }
        }
        public T2 Value2
        {
            get { return value2; }
        }
    }
}