using System;
using System.Collections.Generic;
using System.Text;

namespace R_Tree_Framework.Utility
{
    public interface ISavable
    {
        Byte[] GetBytes();
        Int32 GetSize();
    }
}
