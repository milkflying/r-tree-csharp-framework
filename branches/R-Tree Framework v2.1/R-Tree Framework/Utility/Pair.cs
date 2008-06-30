using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Framework;

namespace R_Tree_Framework.Utility
{
    /// <summary>
    /// The Pair class represents a 2-tuple
    /// </summary>
    /// <typeparam name="T1">Type of the first tuple</typeparam>
    /// <typeparam name="T2">Type of the second tuple</typeparam>
    public class Pair<T1, T2> : UtilityObject
    {
        private T1 _value1;
        private T2 _value2;

        /// <summary>
        /// Construct a 2-tuple of value1 and value2
        /// </summary>
        /// <param name="value1">The first value in the tuple</param>
        /// <param name="value2">The second value in the tuple</param>
        public Pair(T1 value1, T2 value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        /// <summary>
        /// The first value
        /// </summary>
        public virtual T1 Value1
        {
            get { return _value1; }
            protected set { _value1 = value; }
        }
        /// <summary>
        /// The second value
        /// </summary>
        public virtual T2 Value2
        {
            get { return _value2; }
            protected set { _value2 = value; }
        }
    }
}
