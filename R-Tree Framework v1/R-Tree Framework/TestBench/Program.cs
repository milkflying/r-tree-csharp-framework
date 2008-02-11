using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;
using Edu.Psu.Cse.R_Tree_Framework.CacheManagers;
using Edu.Psu.Cse.R_Tree_Framework.Indexes;
using Edu.Psu.Cse.R_Tree_Framework.Performance_Metrics;
using System.IO;

namespace TestBench
{ }
//    public class Program : IDisposable
//    {
//        public static Int32
//            MAXIMUM_OCCUPANCY = Constants.NODE_ENTRIES_PER_NODE,
//            MINIMUM_OCCUPANCY = MAXIMUM_OCCUPANCY *3/10,
//            CACHE_SIZE = 0;

//        public static String
//            DATABASE_LOCATION = "database.dat",
//            DATA_SET_LOCATION_U_S = "uniform_small.dat",
//            DATA_SET_LOCATION_U_L = "uniform_large.dat",
//            DATA_SET_LOCATION_R_S = "real_small.dat",
//            DATA_SET_LOCATION_R_L = "real_large.dat",
//            QUERY_PLAN_LOCATION = "large_q30.dat",
//            QUERY_PLAN_RESULTS_LOCATION_U_S = "query_plan_results\\results_uniform_small.dat",
//            QUERY_PLAN_RESULTS_LOCATION_U_L = "query_plan_results\\results_uniform_large.dat",
//            QUERY_PLAN_RESULTS_LOCATION_R_S = "query_plan_results\\results_real_small.dat",
//            QUERY_PLAN_RESULTS_LOCATION_R_L = "query_plan_results\\results_real_large.dat",
//            COMPARISON_RESULTS_LOCATION_U_S = "uniform_small_q30_mod_out.txt",
//            COMPARISON_RESULTS_LOCATION_U_L = "uniform_large_q30_mod_out.txt",
//            COMPARISON_RESULTS_LOCATION_R_S = "real_small_q30_mod_out.txt",
//            COMPARISON_RESULTS_LOCATION_R_L = "real_large_q30_mod_out.txt",
//            SAVED_INDEX_LOCATION_U_S = "saved_index\\uniform_small_saved_index.dat",
//            SAVED_INDEX_LOCATION_U_L = "saved_index\\uniform_large_saved_index.dat",
//            SAVED_INDEX_LOCATION_R_S = "saved_index\\real_small_saved_index.dat",
//            SAVED_INDEX_LOCATION_R_L = "saved_index\\real_large_saved_index.dat",
//            SAVED_CACHE_LOCATION_U_S = "saved_cache\\uniform_small_saved_cache.dat",
//            SAVED_CACHE_LOCATION_U_L = "saved_cache\\uniform_large_saved_cache.dat",
//            SAVED_CACHE_LOCATION_R_S = "saved_cache\\real_small_saved_cache.dat",
//            SAVED_CACHE_LOCATION_R_L = "saved_cache\\real_large_saved_cache.dat",
//            SAVED_MEMORY_LOCATION_U_S = "saved_memory\\uniform_small_saved_memory.dat",
//            SAVED_MEMORY_LOCATION_U_L = "saved_memory\\uniform_large_saved_memory.dat",
//            SAVED_MEMORY_LOCATION_R_S = "saved_memory\\real_small_saved_memory.dat",
//            SAVED_MEMORY_LOCATION_R_L = "saved_memory\\real_large_saved_memory.dat";

//        public static void Main(string[] args)
//        {
//            BuildIndexs(typeof(R_Tree));
//            RunQueries(typeof(R_Tree));
            
//            BuildIndexs(typeof(R_Star_Tree));
//            RunQueries(typeof(R_Star_Tree));
//            /*
//            CompareResults(typeof(R_Star_Tree));*/
//        }
//        public static void RunQueries(Type tree)
//        {
//            String ext = "";
//            if (tree.Equals(typeof(R_Star_Tree)))
//                ext = "r_star_tree";
//            else if (tree.Equals(typeof(R_Tree)))
//                ext = "r_tree";
//            Program program = new Program(tree, SAVED_INDEX_LOCATION_U_S+ ext, SAVED_CACHE_LOCATION_U_S + ext);
//            program.ExecuteQueryPlan(QUERY_PLAN_LOCATION, QUERY_PLAN_RESULTS_LOCATION_U_S+ ext);
//            program.Dispose();
//            program = new Program(tree, SAVED_INDEX_LOCATION_U_L + ext, SAVED_CACHE_LOCATION_U_L + ext);
//            program.ExecuteQueryPlan(QUERY_PLAN_LOCATION, QUERY_PLAN_RESULTS_LOCATION_U_L+ ext);
//            program.Dispose();
//            program = new Program(tree, SAVED_INDEX_LOCATION_R_S + ext, SAVED_CACHE_LOCATION_R_S + ext);
//            program.ExecuteQueryPlan(QUERY_PLAN_LOCATION, QUERY_PLAN_RESULTS_LOCATION_R_S+ ext);
//            program.Dispose();
//            program = new Program(tree, SAVED_INDEX_LOCATION_R_L + ext, SAVED_CACHE_LOCATION_R_L + ext);
//            program.ExecuteQueryPlan(QUERY_PLAN_LOCATION, QUERY_PLAN_RESULTS_LOCATION_R_L+ ext);
//            program.Dispose();
//            CompareResults(tree);
//        }
//        public static void BuildIndexs(Type tree)
//        {
//            String ext = "";
//            if (tree.Equals(typeof(R_Star_Tree)))
//                ext = "r_star_tree";
//            else if (tree.Equals(typeof(R_Tree)))
//                ext = "r_tree";
//            Program program = new Program(tree);
//            program.BuildIndex(DATA_SET_LOCATION_U_S);
//            program.SaveIndex(SAVED_INDEX_LOCATION_U_S + ext, SAVED_CACHE_LOCATION_U_S + ext, SAVED_MEMORY_LOCATION_U_S + ext);
//            program.Dispose();
//            program = new Program(tree);
//            program.BuildIndex(DATA_SET_LOCATION_U_L);
//            program.SaveIndex(SAVED_INDEX_LOCATION_U_L + ext, SAVED_CACHE_LOCATION_U_L + ext, SAVED_MEMORY_LOCATION_U_L + ext);
//            program.Dispose();
//            program = new Program(tree);
//            program.BuildIndex(DATA_SET_LOCATION_R_S);
//            program.SaveIndex(SAVED_INDEX_LOCATION_R_S + ext, SAVED_CACHE_LOCATION_R_S + ext, SAVED_MEMORY_LOCATION_R_S + ext);
//            program.Dispose();
//            program = new Program(tree);
//            program.BuildIndex(DATA_SET_LOCATION_R_L);
//            program.SaveIndex(SAVED_INDEX_LOCATION_R_L + ext, SAVED_CACHE_LOCATION_R_L + ext, SAVED_MEMORY_LOCATION_R_L + ext);
//            program.Dispose();
//        }
//        public static void CompareResults(Type tree)
//        {
//            String ext = "";
//            if (tree.Equals(typeof(R_Star_Tree)))
//                ext = "r_star_tree";
//            else if (tree.Equals(typeof(R_Tree)))
//                ext = "r_tree";
//            CompareResults("comparison_Results\\comparison_uniform_small.dat" + ext, COMPARISON_RESULTS_LOCATION_U_S, QUERY_PLAN_RESULTS_LOCATION_U_S + ext);
//            CompareResults("comparison_Results\\comparison_uniform_large.dat" + ext, COMPARISON_RESULTS_LOCATION_U_L, QUERY_PLAN_RESULTS_LOCATION_U_L + ext);
//            CompareResults("comparison_Results\\comparison_real_small.dat" + ext, COMPARISON_RESULTS_LOCATION_R_S, QUERY_PLAN_RESULTS_LOCATION_R_S + ext);
//            CompareResults("comparison_Results\\comparison_real_large.dat" + ext, COMPARISON_RESULTS_LOCATION_R_L, QUERY_PLAN_RESULTS_LOCATION_R_L + ext);
//        }
//        public static void CompareResults(String outputLocation, String ken, String mine)
//        {
//            StreamWriter output = new StreamWriter(outputLocation);
//            StreamReader readerKen = new StreamReader(ken),
//                readerMine = new StreamReader(mine);
//            String buffer1, buffer2;
//            while ((buffer1 = readerKen.ReadLine().Trim()).Equals("")) ;
//            while ((buffer2 = readerMine.ReadLine().Trim()).Equals("")) ;
//            while (!readerKen.EndOfStream)
//            {
//                String
//                    queryTypeKen = buffer1,
//                    queryTypeMine = buffer2,
//                    line1Ken = readerKen.ReadLine().Trim(),
//                    line1Mine = readerMine.ReadLine().Trim(),
//                    line2Ken = readerKen.ReadLine().Trim(),
//                    line2Mine = readerMine.ReadLine().Trim();

//                if (!queryTypeKen.Equals(queryTypeMine))
//                    ReportQueryHeaderError(output, queryTypeKen, queryTypeMine, line1Ken, line1Mine, line2Ken, line2Mine, "Inconsistant Query Type", "");
//                /*else if (!line1Ken.Equals(line1Mine))
//                    ReportQueryHeaderError(output, queryTypeKen, queryTypeMine, line1Ken, line1Mine, line2Ken, line2Mine, "Inconsistant Line 1", "");
//                else if (!line2Ken.Equals(line2Mine))
//                    ReportQueryHeaderError(output, queryTypeKen, queryTypeMine, line1Ken, line1Mine, line2Ken, line2Mine, "Inconsistant Line 2", "");
//                */else
//                {
//                    List<Int32> resultsKen = new List<Int32>(),
//                        resultsMine = new List<Int32>();
//                    readerKen.ReadLine();
//                    readerMine.ReadLine();
//                    while (!(buffer1 = readerKen.ReadLine().Trim()).Equals("Complementary Objects:"))
//                        resultsKen.Add(Int32.Parse(buffer1.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]));
//                    while (!(buffer2 = readerMine.ReadLine().Trim()).Equals("Complementary Objects:"))
//                        resultsMine.Add(Int32.Parse(buffer2.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]));
//                    if (queryTypeKen.Equals("Traditional Range Search") || queryTypeKen.Equals("Traditional Window Search"))
//                    {
//                        List<Int32> inKenNotMine = new List<Int32>(),
//                            inMineNotKen = new List<Int32>();
//                        foreach (Int32 result in resultsKen)
//                            if (!resultsMine.Contains(result))
//                                inKenNotMine.Add(result);
//                        foreach (Int32 result in resultsMine)
//                            if (!resultsKen.Contains(result))
//                                inMineNotKen.Add(result);
//                        if (inKenNotMine.Count > 0 || inMineNotKen.Count > 0)
//                        {
//                            String conflictingResults = "Results in Ken's output that are not in new output:" + Environment.NewLine;
//                            foreach (Int32 result in inKenNotMine)
//                                conflictingResults += result.ToString() + Environment.NewLine;
//                            conflictingResults += "Results in new output that are not in Ken's output:" + Environment.NewLine;
//                            foreach (Int32 result in inMineNotKen)
//                                conflictingResults += result.ToString() + Environment.NewLine;
//                            ReportQueryHeaderError(output, queryTypeKen, queryTypeMine, line1Ken, line1Mine, line2Ken, line2Mine, "Inconsistant Query Results", conflictingResults);
//                        }
//                    }
//                    else if (queryTypeKen.Equals("Traditional Nearest Neighbor Search"))
//                    {
//                        for (int i = 0; i < resultsKen.Count -1; i++)
//                            if (i >= resultsMine.Count ||  resultsKen[i] != resultsMine[i])
//                            {
//                                String conflictingResults = "Results in Ken's output:" + Environment.NewLine;
//                                foreach (Int32 result in resultsKen)
//                                    conflictingResults += result.ToString() + Environment.NewLine;
//                                conflictingResults += "Results in new output:" + Environment.NewLine;
//                                foreach (Int32 result in resultsMine)
//                                    conflictingResults += result.ToString() + Environment.NewLine;
//                                ReportQueryHeaderError(output, queryTypeKen, queryTypeMine, line1Ken, line1Mine, line2Ken, line2Mine, "Inconsistant Query Results", conflictingResults);
//                                break;
//                            }

//                    }
//                    else
//                        throw new Exception();
//                }
//                while (!readerKen.EndOfStream && !(buffer1 = readerKen.ReadLine().Trim()).Equals("")) ;
//                while (!readerMine.EndOfStream && !(buffer2 = readerMine.ReadLine().Trim()).Equals("")) ;
//                if (!readerKen.EndOfStream)
//                    buffer1 = readerKen.ReadLine().Trim();
//                if (!readerMine.EndOfStream)
//                    buffer2 = readerMine.ReadLine().Trim();
//            }
//            readerKen.Close();
//            output.Close();
//            readerMine.Close();
//        }

//        private static void ReportQueryHeaderError(StreamWriter output, 
//            String queryTypeKen, String queryTypeMine, String line1Ken, 
//            String line1Mine, String line2Ken, String line2Mine, 
//            String errorType, String other)
//        {
//            output.WriteLine(errorType);
//            output.WriteLine(queryTypeKen);
//            output.WriteLine(line1Ken);
//            output.WriteLine(line2Ken);

//            output.WriteLine(queryTypeMine);
//            output.WriteLine(line1Mine);
//            output.WriteLine(line2Mine);
//            output.WriteLine(other);
//            output.WriteLine();
//        }
//        private String queryPlan;
//        private LRUCacheManager cache;
//        private PerformanceAnalyzer analyzer;
//        private R_Tree index;
//        public Program(Type tree)
//        {
//            cache = new LRUCacheManager(DATABASE_LOCATION, Constants.PAGE_SIZE, CACHE_SIZE);
//            analyzer = new PerformanceAnalyzer(cache);
//            if (tree.Equals(typeof(R_Star_Tree)))
//                index = new R_Star_Tree(MINIMUM_OCCUPANCY, MAXIMUM_OCCUPANCY, cache);
//            else if (tree.Equals(typeof(R_Tree)))
//                index = new R_Tree(MINIMUM_OCCUPANCY, MAXIMUM_OCCUPANCY, cache);
//        }

//        public Program(Type tree, String indexLoc, String cacheLoc)
//        {
//            cache = new LRUCacheManager(cacheLoc, CACHE_SIZE);
//            analyzer = new PerformanceAnalyzer(cache);
//            if (tree.Equals(typeof(R_Star_Tree)))
//                index = new R_Star_Tree(MINIMUM_OCCUPANCY, MAXIMUM_OCCUPANCY, cache);
//            else if (tree.Equals(typeof(R_Tree))) 
//                index = new R_Tree(indexLoc, cache);
//        }

//        public void BuildIndex(String dataFileLocation)
//        {
//            StreamReader reader = new StreamReader(dataFileLocation);
//            while (!reader.EndOfStream)
//            {
//                String[] values = reader.ReadLine().Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
//                Int32 recordId = Int32.Parse(values[0]);
//                Single x = Single.Parse(values[1]), y = Single.Parse(values[2]);
//                Record record = new Record(recordId, new MinimumBoundingBox(x, y, x, y));
//                index.Insert(record);
//            }
//            reader.Close();
//        }
//        public void SaveIndex(String indexLoc, String cacheLoc, String memLoc)
//        {
//            index.SaveIndex(indexLoc, cacheLoc, memLoc);
//        }
//        public void ExecuteQueryPlan(String queryPlan, String resultLocation)
//        {
//            //index.ForceMBBUpdate();
//            analyzer.ClearStatistics();
//            cache.FlushCache();
//            StreamReader reader = new StreamReader(queryPlan);
//            StreamWriter writer = new StreamWriter(resultLocation);
//            while (!reader.EndOfStream)
//            {
//                String[] values = reader.ReadLine().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
//                Int32 queryNumber = Int32.Parse(values[0]);
//                Char queryType = Char.Parse(values[1]);
//                if (queryType == 'Q')
//                {
//                    writer.WriteLine();
//                    Query query;
//                    Char searchType = Char.Parse(values[2]);
//                    switch (searchType)
//                    {
//                        case 'R':
//                            {
//                                Single x = Single.Parse(values[3]), y = Single.Parse(values[4]), r = Single.Parse(values[5]);
//                                query = new RangeQuery(x, y, r);
//                                writer.WriteLine("Traditional Range Search");
//                                writer.WriteLine("Point: ({0:F2},{1:F2})", x, y);
//                                writer.WriteLine("Range: {0:F2}", r);
//                                break;
//                            }
//                        case 'W':
//                            {
//                                Single x = Single.Parse(values[3]), y = Single.Parse(values[4]), r = Single.Parse(values[5]);
//                                query = new WindowQuery(x - r, y - r, x, y);
//                                writer.WriteLine("Traditional Window Search");
//                                writer.WriteLine("Lower Point: ({0:F2},{1:F2})", x - r, y - r);
//                                writer.WriteLine("Upper Point: ({0:F2},{1:F2})", x, y);
//                                break;
//                            }
//                        case 'K':
//                            {
//                                Single x = Single.Parse(values[3]), y = Single.Parse(values[4]);
//                                Int32 k = Int32.Parse(values[5]);
//                                query = new KNearestNeighborQuery(k, x, y);
//                                writer.WriteLine("Traditional Nearest Neighbor Search");
//                                writer.WriteLine("Point: ({0:F2},{1:F2})", x, y);
//                                writer.WriteLine("Items: {0}", k);
//                                break;
//                            }
//                        default: continue;
//                    }
//                    List<Record> results = index.Search(query);
//                    writer.WriteLine("Result Objects:");
//                    foreach (Record result in results)
//                    {
//                        writer.WriteLine("{0} {1:F2} {2:F2}", result.RecordID + 1000000 - 1, result.BoundingBox.MinX, result.BoundingBox.MinY);
//                    }
//                    writer.WriteLine("Complementary Objects:");
//                    writer.WriteLine("Statistics");
//                    writer.WriteLine("pload:	 {0}	pwrite:	 {1}", analyzer.PageFaults, analyzer.PageWrites);
//                    analyzer.ClearStatistics();
//                    cache.FlushCache();
//                }
//            }
//            reader.Close();
//            writer.Close();
//        }
//        public virtual void Dispose()
//        {
//            cache.Dispose();
//        }
//    }
//}
