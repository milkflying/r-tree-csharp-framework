using System;
using System.Collections.Generic;
using System.IO;

namespace ExperimentRunner
{
    class Program
    {
        
        static void Main(String[] args)
        {
            String buildIndexLocation = @"C:\Users\Mike\Documents\R-Tree Framework\trunk\IndexBuildsNonRSharp.csv",
                workPlanLocation = @"C:\Users\Mike\Documents\R-Tree Framework\trunk\WorkPlansNonRSharp.csv";
            ExperimentRunner er = new ExperimentRunner();
            er.LoadBuildIndexExperiments(buildIndexLocation);
            er.LoadRunWorkPlanExperiments(workPlanLocation);
            buildIndexLocation = @"C:\Users\Mike\Documents\R-Tree Framework\trunk\IndexBuildsNonRSharp2.csv";
                workPlanLocation = @"C:\Users\Mike\Documents\R-Tree Framework\trunk\WorkPlansNonRSharp2.csv";
            //er.LoadBuildIndexExperiments(buildIndexLocation);
            //er.LoadRunWorkPlanExperiments(workPlanLocation);
            er.RunExperiments();
        }
    }
}