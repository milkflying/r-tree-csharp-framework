using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Edu.Psu.Cse.R_Tree_Framework.Framework;
using Edu.Psu.Cse.R_Tree_Framework.Performance_Metrics;

namespace QueryPlanExecutor
{
    public class QueryPlanExecutor
    {
        #region Instance Variables

        protected String queryPlanFileLocation, resultSaveFileLocation;
        protected PerformanceAnalyzer performanceAnalyzer;
        protected CacheManager cache;
        protected Index treeIndex;

        #endregion
        #region Properties

        protected virtual CacheManager Cache
        {
            get { return cache; }
            set { cache = value; }
        }
        protected virtual PerformanceAnalyzer PerformanceAnalyzer
        {
            get { return performanceAnalyzer; }
            set { performanceAnalyzer = value; }
        }
        protected virtual String QueryPlanFileLocation
        {
            get { return queryPlanFileLocation; }
            set { queryPlanFileLocation = value; }
        }
        protected virtual String ResultSaveFileLocation
        {
            get { return resultSaveFileLocation; }
            set { resultSaveFileLocation = value; }
        }
        protected virtual Index TreeIndex
        {
            get { return treeIndex; }
            set { treeIndex = value; }
        }

        #endregion
        #region Constructors

        public QueryPlanExecutor(
            String queryPlanFileLocation, 
            String resultSaveFileLocation, 
            String savedCacheFileLocation, 
            String savedIndexFileLocation, 
            String databaseRunFileLocation,
            Type cacheType, 
            Type treeType)
        {
            QueryPlanFileLocation = queryPlanFileLocation;
            ResultSaveFileLocation = resultSaveFileLocation;
            SavedCacheBuilder cacheBuilder = new SavedCacheBuilder(cacheType, savedCacheFileLocation, databaseRunFileLocation);
            cacheBuilder.BuildCache();
            Cache = cacheBuilder.Cache;
            SavedIndexBuilder indexBuilder = new SavedIndexBuilder(treeType, Cache, savedIndexFileLocation);
            indexBuilder.BuildIndex();
            TreeIndex = indexBuilder.TreeIndex;
            PerformanceAnalyzer = new PerformanceAnalyzer(Cache);
        }

        #endregion
        #region Public Methods

        public void ExecuteQueryPlan()
        {
            StreamReader reader = new StreamReader(QueryPlanFileLocation);
            StreamWriter writer = new StreamWriter(ResultSaveFileLocation);

            while (!reader.EndOfStream)
            {
                PerformanceAnalyzer.ClearStatistics();
                Cache.FlushCache();
                String[] values = reader.ReadLine().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Int32 queryNumber = Int32.Parse(values[0]);
                if (queryNumber % 100 == 0)
                    Console.WriteLine(queryNumber.ToString());
                Char queryType = Char.Parse(values[1]);
                if (queryType == 'Q')
                {
                    writer.WriteLine();
                    Query query;
                    Char searchType = Char.Parse(values[2]);
                    switch (searchType)
                    {
                        case 'R':
                            {
                                Single x = Single.Parse(values[3]), y = Single.Parse(values[4]), r = Single.Parse(values[5]);
                                query = new RangeQuery(x, y, r);
                                writer.WriteLine("Traditional Range Search");
                                writer.WriteLine("Point: ({0:F2},{1:F2})", x, y);
                                writer.WriteLine("Range: {0:F2}", r);
                                break;
                            }
                        case 'W':
                            {
                                Single x = Single.Parse(values[3]), y = Single.Parse(values[4]), r = Single.Parse(values[5]);
                                query = new WindowQuery(x - r, y - r, x, y);
                                writer.WriteLine("Traditional Window Search");
                                writer.WriteLine("Lower Point: ({0:F2},{1:F2})", x - r, y - r);
                                writer.WriteLine("Upper Point: ({0:F2},{1:F2})", x, y);
                                break;
                            }
                        case 'K':
                            {
                                Single x = Single.Parse(values[3]), y = Single.Parse(values[4]);
                                Int32 k = Int32.Parse(values[5]);
                                query = new KNearestNeighborQuery(k, x, y);
                                writer.WriteLine("Traditional Nearest Neighbor Search");
                                writer.WriteLine("Point: ({0:F2},{1:F2})", x, y);
                                writer.WriteLine("Items: {0}", k);
                                break;
                            }
                        default: continue;
                    }
                    List<Record> results = TreeIndex.Search(query);
                    writer.WriteLine("Result Objects:");
                    foreach (Record result in results)
                    {
                        writer.WriteLine("{0} {1:F2} {2:F2}", result.RecordID + 1000000 - 1, result.BoundingBox.MinX, result.BoundingBox.MinY);
                    }
                    writer.WriteLine("Complementary Objects:");
                    writer.WriteLine("Statistics");
                    writer.WriteLine("pload:	 {0}	pwrite:	 {1}", PerformanceAnalyzer.PageFaults, PerformanceAnalyzer.PageWrites);
                }
            }
            reader.Close();
            writer.Close();
        }

        #endregion
    }
}
