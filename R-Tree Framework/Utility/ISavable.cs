using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Framework;

namespace R_Tree_Framework.Utility
{
    /// <summary>
    /// This interface marks a class as being able to be saved as
    /// a sequence of bytes for either communication or file storage.
    /// </summary>
    public interface ISavable : RTreeFramework_Interface
    {
        Byte[] GetBytes();
        Int32 GetSize();
    }
}
