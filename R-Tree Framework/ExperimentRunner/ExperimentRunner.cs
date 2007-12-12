using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using Edu.Psu.Cse.R_Tree_Framework.CacheManagers;
using Edu.Psu.Cse.R_Tree_Framework.Indexes;

namespace ExperimentRunner
{
    public class ExperimentRunner
    {
        protected static String
            INDEX_BUILDER = @"..\..\..\IndexBuilder\bin\Release\IndexBuilder.exe",
            WORK_PLAN_RUNNER = @"..\..\..\QueryPlanExecutor\bin\Debug\QueryPlanExecutor.exe";

        protected List<Experiment> experiments;

        protected virtual List<Experiment> Experiments
        {
            get { return experiments; }
            set { experiments = value; }
        }

        public ExperimentRunner()
        {
            Experiments = new List<Experiment>();
        }

        public virtual void LoadBuildIndexExperiments(String fileLocation)
        {
            StreamReader reader = new StreamReader(fileLocation);
            while (!reader.EndOfStream)
            {
                String[] values = reader.ReadLine().Split(',');
                BuildIndex experiment = new BuildIndex(
                    (DataSet)Enum.Parse(typeof(DataSet), values[0], true),
                    (Cardinality)Enum.Parse(typeof(Cardinality), values[1], true),
                    (IndexType)Enum.Parse(typeof(IndexType), values[2], true),
                    (ReservationBufferSize)Enum.Parse(typeof(ReservationBufferSize), values[3], true),
                    (CacheType)Enum.Parse(typeof(CacheType), values[4], true),
                    (CacheSize)Enum.Parse(typeof(CacheSize), values[5], true));
                Experiments.Add(experiment);
            }
            reader.Close();
        }
        public virtual void LoadRunWorkPlanExperiments(String fileLocation)
        {
            StreamReader reader = new StreamReader(fileLocation);
            while (!reader.EndOfStream)
            {
                String[] values = reader.ReadLine().Split(',');
                RunWorkPlan experiment = new RunWorkPlan(
                    (DataSet)Enum.Parse(typeof(DataSet), values[0], true),
                    (Cardinality)Enum.Parse(typeof(Cardinality), values[1], true),
                    (IndexType)Enum.Parse(typeof(IndexType), values[2], true),
                    (ReservationBufferSize)Enum.Parse(typeof(ReservationBufferSize), values[3], true),
                    (CacheSize)Enum.Parse(typeof(CacheSize), values[4]),
                    (Drive)Enum.Parse(typeof(Drive), values[5]),
                    (CacheType)Enum.Parse(typeof(CacheType), values[6]),
                    (WorkPlan)Enum.Parse(typeof(WorkPlan), values[7]));
                Experiments.Add(experiment);
            }
            reader.Close();
        }

        public virtual void RunExperiments()
        {
            Int32 k = 1;
            foreach (Experiment experiment in Experiments)
                if (experiment is BuildIndex)
                {
                    Console.WriteLine("Running Experiment: {0}", k++);
                        RunExperiment(experiment as BuildIndex);
                }
                else
                {
                    Console.WriteLine("Running Experiment: {0}", k++);
                        RunExperiment(experiment as RunWorkPlan);
                }
        }
        protected virtual void RunExperiment(BuildIndex experiment)
        {
            Process indexBuilder = new Process();
            indexBuilder.StartInfo.Arguments = String.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\" \"{7}\" \"{8}\"",
                GetCacheType(experiment.CacheType),
                GetIndexType(experiment.IndexType),
                experiment.DatabaseSaveLocation,
                experiment.DataSetLocation,
                experiment.IndexSaveLocation,
                experiment.CacheSaveLocation,
                experiment.MemorySaveLocation,
                GetCacheSize(experiment.CacheSize),
                GetReservationBufferSize(experiment.ReservationBufferSize));
            indexBuilder.StartInfo.FileName = INDEX_BUILDER;
           // Console.WriteLine("Building index with arguments: {0}", indexBuilder.StartInfo.Arguments);
            indexBuilder.Start();
            indexBuilder.WaitForExit();
        }
        protected virtual void RunExperiment(RunWorkPlan experiment)
        {
            Process workPlanRunner = new Process();
            workPlanRunner.StartInfo.Arguments = String.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\"",
                experiment.WorkPlanLocation,
                experiment.ResultsSaveLocation,
                experiment.CacheSaveLocation,
                experiment.IndexSaveLocation,
                experiment.DatabaseRunLocation,
                GetCacheType(experiment.CacheType),
                GetIndexType(experiment.IndexType));
            workPlanRunner.StartInfo.FileName = WORK_PLAN_RUNNER;
            //Console.WriteLine("Running work plan with arguments: {0}", workPlanRunner.StartInfo.Arguments);
            workPlanRunner.Start();
            workPlanRunner.WaitForExit();
        }
        protected virtual String GetCacheType(CacheType cacheType)
        {
            switch (cacheType)
            {
                case CacheType.HighestTreeLevel:
                    return typeof(HighestTreeLevelCacheManager).AssemblyQualifiedName;
                case CacheType.LevelProportional:
                    return typeof(LevelProportionalCacheManager).AssemblyQualifiedName;
                case CacheType.LRU:
                    return typeof(LRUCacheManager).AssemblyQualifiedName;
                case CacheType.None:
                    return typeof(LRUCacheManager).AssemblyQualifiedName;
                default:
                    throw new Exception("Illegal Cache Type");
            }
        }
        protected virtual String GetIndexType(IndexType indexType)
        {
            switch (indexType)
            {
                case IndexType.Flash_R_Tree:
                    return typeof(Flash_R_Tree).AssemblyQualifiedName;
                case IndexType.Flash_R_Tree_Extended:
                    return typeof(Flash_R_Tree_Extended).AssemblyQualifiedName;
                case IndexType.R_Sharp_Tree:
                    return typeof(R_Sharp_Tree).AssemblyQualifiedName;
                case IndexType.R_Star_Tree:
                    return typeof(R_Star_Tree).AssemblyQualifiedName;
                default:
                    throw new Exception("Illegal Index Type");
            }
        }
        protected virtual String GetCacheSize(CacheSize cacheSize)
        {
            switch (cacheSize)
            {
                case CacheSize.Large:
                    return "10";
                case CacheSize.Medium:
                    return "5";
                case CacheSize.None:
                    return "0";
                default:
                    throw new Exception("Illegal Cache Size");
            }
        }
        protected virtual String GetReservationBufferSize(ReservationBufferSize reservationBufferSize)
        {
            switch (reservationBufferSize)
            {
                case ReservationBufferSize.Large:
                    return "64";
                case ReservationBufferSize.Medium:
                    return "16";
                case ReservationBufferSize.Small:
                    return "4";
                case ReservationBufferSize.None:
                    return "0";
                default:
                    throw new Exception("Illegal Reservation Buffer Size");
            }
        }

    }
}
