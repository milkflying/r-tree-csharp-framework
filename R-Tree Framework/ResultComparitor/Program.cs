using System;
using System.Collections.Generic;
using System.Text;

namespace ResultComparitor
{
    internal class Program
    {
        internal static void Main(String[] args)
        {
            String
                baseResults = args[0],
                newResults = args[1],
                comparisonResults = args[2];
            ResultsComparitor
                resultsComparitor = new ResultsComparitor(
                    baseResults, 
                    newResults,
                    comparisonResults);
            resultsComparitor.CompareResults();
        }
    }
}
