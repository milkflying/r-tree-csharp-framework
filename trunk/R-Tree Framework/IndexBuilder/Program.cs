using System;
using System.Collections.Generic;
using System.Text;

namespace IndexBuilder
{
    internal class Program
    {
        internal static void Main(String[] args)
        {
            Type
                cacheType = Type.GetType(args[0]),
                treeType = Type.GetType(args[1]);
            String 
                databaseFileLocation = args[2],
                dataSetFileLocation = args[3],
                indexSaveFileLocation = args[4],
                cacheSaveFileLocation = args[5],
                memorySaveFileLocation = args[6];
            Int32 
                pageSize = Int32.Parse(args[7]),
                cacheSize = Int32.Parse(args[8]),
                minimumNodeOccupancy = Int32.Parse(args[9]),
                maximumNodeOccupancy = Int32.Parse(args[10]);

            CacheBuilder 
                cacheBuilder = new CacheBuilder(
                    cacheType,
                    databaseFileLocation,
                    pageSize, 
                    cacheSize);
            cacheBuilder.BuildCache();

            IndexBuilder 
                indexBuilder = new IndexBuilder(
                    treeType,
                    minimumNodeOccupancy,
                    maximumNodeOccupancy,
                    cacheBuilder.Cache,
                    dataSetFileLocation,
                    indexSaveFileLocation,
                    cacheSaveFileLocation,
                    memorySaveFileLocation);
            indexBuilder.BuildIndex();
        }
    }
}
