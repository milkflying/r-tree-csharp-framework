using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public interface PageData
    {
        Address Address { get; }
        Byte[] GeneratePageData();
    }
}
