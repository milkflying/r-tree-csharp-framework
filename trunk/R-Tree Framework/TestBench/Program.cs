using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;
using Edu.Psu.Cse.R_Tree_Framework.CacheManagers;
using Edu.Psu.Cse.R_Tree_Framework.Indexes;
using Edu.Psu.Cse.R_Tree_Framework.Performance_Metrics;
using System.IO;

namespace TestBench
{
    public class Program : IDisposable
    {
        public static Int32
            MAXIMUM_OCCUPANCY = Constants.NODE_ENTRIES_PER_NODE,
            MINIMUM_OCCUPANCY = MAXIMUM_OCCUPANCY *3/10,
            CACHE_SIZE = 0;

        public static String
            DATABASE_LOCATION = "database.dat",
            DATA_SET_LOCATION_U_S = "uniform_small.dat",
            DATA_SET_LOCATION_U_L = "uniform_large.dat",
            DATA_SET_LOCATION_R_S = "real_small.dat",
            DATA_SET_LOCATION_R_L = "real_large.dat",
            QUERY_PLAN_LOCATION = "large_q30.dat",
            QUERY_PLAN_RESULTS_LOCATION_U_S = "results_uniform_small.dat",
            QUERY_PLAN_RESULTS_LOCATION_U_L = "results_uniform_large.dat",
            QUERY_PLAN_RESULTS_LOCATION_R_S = "results_real_small.dat",
            QUERY_PLAN_RESULTS_LOCATION_R_L = "results_real_large.dat",
            COMPARISON_RESULTS_LOCATION_U_S = "uniform_small_q30_mod_out.txt",
            COMPARISON_RESULTS_LOCATION_U_L = "uniform_large_q30_mod_out.txt",
            COMPARISON_RESULTS_LOCATION_R_S = "real_small_q30_mod_out.txt",
            COMPARISON_RESULTS_LOCATION_R_L = "real_large_q30_mod_out.txt";

        public static void Main(string[] args)
        {
            Program program = new Program();
            //Program program = new Program(DATA_SET_LOCATION, "savedIndex.dat", "savedCache.dat");
            program.BuildIndex(DATA_SET_LOCATION_U_S);
            program.ExecuteQueryPlan(QUERY_PLAN_LOCATION, QUERY_PLAN_RESULTS_LOCATION_U_S);
            program.Dispose();
            
            program = new Program();
            //Program program = new Program(DATA_SET_LOCATION, "savedIndex.dat", "savedCache.dat");
            program.BuildIndex(DATA_SET_LOCATION_U_L);
            program.ExecuteQueryPlan(QUERY_PLAN_LOCATION, QUERY_PLAN_RESULTS_LOCATION_U_L);
            program.Dispose();

            program = new Program();
            //Program program = new Program(DATA_SET_LOCATION, "savedIndex.dat", "savedCache.dat");
            program.BuildIndex(DATA_SET_LOCATION_R_S);
            program.ExecuteQueryPlan(QUERY_PLAN_LOCATION, QUERY_PLAN_RESULTS_LOCATION_R_S);
            program.Dispose();

            program = new Program();
            //Program program = new Program(DATA_SET_LOCATION, "savedIndex.dat", "savedCache.dat");
            program.BuildIndex(DATA_SET_LOCATION_R_L);
            program.ExecuteQueryPlan(QUERY_PLAN_LOCATION, QUERY_PLAN_RESULTS_LOCATION_R_L);
            program.Dispose();
            CompareResults("comparison.dat");
        }

        public static void CompareResults(String outputLocation)
        {
            StreamWriter writer = new StreamWriter(outputLocation);
            CompareResults(writer, COMPARISON_RESULTS_LOCATION_U_S, QUERY_PLAN_RESULTS_LOCATION_U_S);
            /*CompareResults(writer, COMPARISON_RESULTS_LOCATION_U_L, QUERY_PLAN_RESULTS_LOCATION_U_L);
            CompareResults(writer, COMPARISON_RESULTS_LOCATION_R_S, QUERY_PLAN_RESULTS_LOCATION_R_S);
            CompareResults(writer, COMPARISON_RESULTS_LOCATION_R_L, QUERY_PLAN_RESULTS_LOCATION_R_L);*/
            writer.Close();
        }
        public static void CompareResults(StreamWriter output, String ken, String mine)
        {
            StreamReader readerKen = new StreamReader(ken),
                readerMine = new StreamReader(mine);
            String buffer1, buffer2;
            while ((buffer1 = readerKen.ReadLine().Trim()).Equals("")) ;
            while ((buffer2 = readerMine.ReadLine().Trim()).Equals("")) ;
            while (!readerKen.EndOfStream)
            {
                String
                    queryTypeKen = buffer1,
                    queryTypeMine = buffer2,
                    line1Ken = readerKen.ReadLine().Trim(),
                    line1Mine = readerMine.ReadLine().Trim(),
                    line2Ken = readerKen.ReadLine().Trim(),
                    line2Mine = readerMine.ReadLine().Trim();

                if (!queryTypeKen.Equals(queryTypeMine))
                    ReportQueryHeaderError(output, queryTypeKen, queryTypeMine, line1Ken, line1Mine, line2Ken, line2Mine, "Inconsistant Query Type", "");
                /*else if (!line1Ken.Equals(line1Mine))
                    ReportQueryHeaderError(output, queryTypeKen, queryTypeMine, line1Ken, line1Mine, line2Ken, line2Mine, "Inconsistant Line 1", "");
                else if (!line2Ken.Equals(line2Mine))
                    ReportQueryHeaderError(output, queryTypeKen, queryTypeMine, line1Ken, line1Mine, line2Ken, line2Mine, "Inconsistant Line 2", "");
                */else
                {
                    List<Int32> resultsKen = new List<Int32>(),
                        resultsMine = new List<Int32>();
                    readerKen.ReadLine();
                    readerMine.ReadLine();
                    while (!(buffer1 = readerKen.ReadLine().Trim()).Equals("Complementary Objects:"))
                        resultsKen.Add(Int32.Parse(buffer1.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]));
                    while (!(buffer2 = readerMine.ReadLine().Trim()).Equals("Complementary Objects:"))
                        resultsMine.Add(Int32.Parse(buffer2.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]));
                    if (queryTypeKen.Equals("Traditional Range Search") || queryTypeKen.Equals("Traditional Window Search"))
                    {
                        List<Int32> inKenNotMine = new List<Int32>(),
                            inMineNotKen = new List<Int32>();
                        foreach (Int32 result in resultsKen)
                            if (!resultsMine.Contains(result))
                                inKenNotMine.Add(result);
                        foreach (Int32 result in resultsMine)
                            if (!resultsKen.Contains(result))
                                inMineNotKen.Add(result);
                        if (inKenNotMine.Count > 0 || inMineNotKen.Count > 0)
                        {
                            String conflictingResults = "Results in Ken's output that are not in new output:" + Environment.NewLine;
                            foreach (Int32 result in inKenNotMine)
                                conflictingResults += result.ToString() + Environment.NewLine;
                            conflictingResults += "Results in new output that are not in Ken's output:" + Environment.NewLine;
                            foreach (Int32 result in inMineNotKen)
                                conflictingResults += result.ToString() + Environment.NewLine;
                            ReportQueryHeaderError(output, queryTypeKen, queryTypeMine, line1Ken, line1Mine, line2Ken, line2Mine, "Inconsistant Query Results", conflictingResults);
                        }
                    }
                    else if (queryTypeKen.Equals("Traditional Nearest Neighbor Search"))
                    {
                        for (int i = 0; i < resultsKen.Count; i++)
                            if (resultsKen[i] != resultsMine[i])
                            {
                                String conflictingResults = "Results in Ken's output:" + Environment.NewLine;
                                foreach (Int32 result in resultsKen)
                                    conflictingResults += result.ToString() + Environment.NewLine;
                                conflictingResults += "Results in new output:" + Environment.NewLine;
                                foreach (Int32 result in resultsMine)
                                    conflictingResults += result.ToString() + Environment.NewLine;
                                ReportQueryHeaderError(output, queryTypeKen, queryTypeMine, line1Ken, line1Mine, line2Ken, line2Mine, "Inconsistant Query Results", conflictingResults);
                                break;
                            }
                    }
                    else
                        throw new Exception();
                }
                while (!readerKen.EndOfStream && !(buffer1 = readerKen.ReadLine().Trim()).Equals("")) ;
                while (!readerMine.EndOfStream && !(buffer2 = readerMine.ReadLine().Trim()).Equals("")) ;
                if (!readerKen.EndOfStream)
                    buffer1 = readerKen.ReadLine().Trim();
                if (!readerMine.EndOfStream)
                    buffer2 = readerMine.ReadLine().Trim();
            }
            readerKen.Close();
            readerMine.Close();
        }

        private static void ReportQueryHeaderError(StreamWriter output, 
            String queryTypeKen, String queryTypeMine, String line1Ken, 
            String line1Mine, String line2Ken, String line2Mine, 
            String errorType, String other)
        {
            output.WriteLine(errorType);
            output.WriteLine(queryTypeKen);
            output.WriteLine(line1Ken);
            output.WriteLine(line2Ken);

            output.WriteLine(queryTypeMine);
            output.WriteLine(line1Mine);
            output.WriteLine(line2Mine);
            output.WriteLine(other);
            output.WriteLine();
        }
        private String queryPlan;
        private LRUCacheManager cache;
        private PerformanceAnalyzer analyzer;
        private R_Tree index;
        public Program()
        {
            cache = new LRUCacheManager(DATABASE_LOCATION, Constants.PAGE_SIZE, CACHE_SIZE);
            analyzer = new PerformanceAnalyzer(cache);
            index = new R_Tree(MINIMUM_OCCUPANCY, MAXIMUM_OCCUPANCY, cache);
        }

        public Program(String indexLoc, String cacheLoc)
        {
            cache = new LRUCacheManager(cacheLoc, CACHE_SIZE);
            analyzer = new PerformanceAnalyzer(cache);
            index = new R_Tree(indexLoc, cache);
        }

        public void BuildIndex(String dataFileLocation)
        {
            StreamReader reader = new StreamReader(dataFileLocation);
            Single maxX = 0, maxY = 0;
            while (!reader.EndOfStream)
            {
                String[] values = reader.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                Int32 recordId = Int32.Parse(values[0]);
                Single x = Single.Parse(values[1]), y = Single.Parse(values[2]);
                if (x > maxX)
                    maxX = x;
                if (y > maxY)
                    maxY = y;
                Record record = new Record(recordId, new MinimumBoundingBox(x, y, x, y));
                index.Insert(record);
            }
            reader.Close();
            //index.SaveIndex("savedIndex.dat", "savedCache.dat", "savedMemory.dat");
        }
        public void ExecuteQueryPlan(String queryPlan, String resultLocation)
        {
            //index.ForceMBBUpdate();
            analyzer.ClearStatistics();
            cache.FlushCache();
            StreamReader reader = new StreamReader(queryPlan);
            StreamWriter writer = new StreamWriter(resultLocation);
            while (!reader.EndOfStream)
            {
                String[] values = reader.ReadLine().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Int32 queryNumber = Int32.Parse(values[0]);
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
                    List<Record> results = index.Search(query);
                    writer.WriteLine("Result Objects:");
                    foreach (Record result in results)
                    {
                        writer.WriteLine("{0} {1:F2} {2:F2}", result.RecordID + 1000000 - 1, result.BoundingBox.MinX, result.BoundingBox.MinY);
                    }
                    writer.WriteLine("Complementary Objects:");
                    writer.WriteLine("Statistics");
                    writer.WriteLine("pload:	 {0}	pwrite:	 {1}", analyzer.PageFaults, analyzer.PageWrites);
                    analyzer.ClearStatistics();
                    cache.FlushCache();
                }
            }
            reader.Close();
            writer.Close();
        }
        public virtual void Dispose()
        {
            cache.Dispose();
        }
    }
}
