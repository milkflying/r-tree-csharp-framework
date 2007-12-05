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
    public class Program
    {
        public static Int32
            MINIMUM_OCCUPANCY = MAXIMUM_OCCUPANCY / 3,
            MAXIMUM_OCCUPANCY = Constants.NODE_ENTRIES_PER_NODE,
            CACHE_SIZE = 0;

        public static String 
            DATABASE_LOCATION = "database.dat",
            DATA_SET_LOCATION = "uniform_small.dat",
            QUERY_PLAN_LOCATION = "large_q30.dat",
            QUERY_PLAN_RESULTS_LOCATION = "results.dat";

        public static void Main(string[] args)
        {
            Program program = new Program(QUERY_PLAN_LOCATION);
            program.BuildIndex(DATA_SET_LOCATION);
            program.ExecuteQueryPlan();
        }

        private String queryPlan;
        private LRUCacheManager cache;
        private PerformanceAnalyzer analyzer;
        private R_Tree index;
        public Program(String queryPlan)
        {
            this.queryPlan = queryPlan;
            cache = new LRUCacheManager(DATABASE_LOCATION, Constants.PAGE_SIZE, CACHE_SIZE);
            analyzer = new PerformanceAnalyzer(cache);
            index = new R_Tree(MINIMUM_OCCUPANCY, MAXIMUM_OCCUPANCY, cache);
        }

        public void BuildIndex(String dataFileLocation)
        {
            StreamReader reader = new StreamReader(dataFileLocation);
            while (!reader.EndOfStream)
            {
                String[] values = reader.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                Int32 recordId = Int32.Parse(values[0]);
                Double x = Double.Parse(values[1]), y = Double.Parse(values[2]);
                Record record = new Record(recordId, new MinimumBoundingBox(x, y, x, y));
                index.Insert(record);
            }
            reader.Close();
        }
        public void ExecuteQueryPlan()
        {
            StreamReader reader = new StreamReader(QUERY_PLAN_LOCATION);
            StreamWriter writer = new StreamWriter(QUERY_PLAN_RESULTS_LOCATION);
            while (!reader.EndOfStream)
            {
                String[] values = reader.ReadLine().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Int32 queryNumber = Int32.Parse(values[0]);
                Char queryType = Char.Parse(values[1]);
                if (queryType == 'Q')
                {
                    Query query;
                    Char searchType = Char.Parse(values[2]);
                    switch(searchType)
                    {
                        case 'R':
                            {
                                Double x = Double.Parse(values[3]), y = Double.Parse(values[4]), r = Double.Parse(values[5]);
                                query = new RangeQuery(x, y, r);
                                break;
                            }
                        case 'W':
                            {
                                Double x = Double.Parse(values[3]), y = Double.Parse(values[4]), r = Double.Parse(values[5]);
                                query = new WindowQuery(x - r, y - r, x + r, y + r);
                                break;
                            }
                        case 'K':
                            {
                                Double x = Double.Parse(values[3]), y = Double.Parse(values[4]);
                                Int32 k = Int32.Parse(values[5]);
                                query = new KNearestNeighborQuery(k, x, y);
                                break;
                            }
                        default: continue;
                    }
                    List<Record> results = index.Search(query);
                    writer.WriteLine("Q" + queryNumber);
                    foreach (Record result in results)
                    {
                        writer.WriteLine(result.RecordID);
                    }
                }
            }
            reader.Close();
            writer.Close();
        }
    }
}
