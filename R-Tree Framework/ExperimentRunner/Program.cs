using System;
using System.Collections.Generic;
using System.IO;

namespace ExperimentRunner
{
    class Program
    {
        
        static void Main(String[] args)
        {
            String buildIndexLocation = @"C:\Users\Mike\Documents\R-Tree Framework\trunk\IndexBuilds.csv";
            ExperimentRunner er = new ExperimentRunner();
            er.LoadBuildIndexExperiments(buildIndexLocation);
            er.RunExperiments();
        }
    }
}