using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ResultComparitor
{
    public class ResultsComparitor
    {
        #region Instance Variables

        protected String
            baseResultsFileLocation,
            newResultsFileLocation, 
            comparisonFileLocation;

        #endregion
        #region Properties

        protected virtual String BaseResultsFileLocation
        {
            get { return baseResultsFileLocation; }
            set { baseResultsFileLocation = value; }
        }
        protected virtual String NewResultsFileLocation
        {
            get { return newResultsFileLocation; }
            set { newResultsFileLocation = value; }
        }
        protected virtual String ComparisonFileLocation
        {
            get { return comparisonFileLocation; }
            set { comparisonFileLocation = value; }
        }

        #endregion
        #region Constructors

        public ResultsComparitor(
            String baseResultsFileLocation, 
            String newResultsFileLocation, 
            String comparisonFileLocation)
        {
            BaseResultsFileLocation = baseResultsFileLocation;
            NewResultsFileLocation = newResultsFileLocation;
            ComparisonFileLocation = comparisonFileLocation;
        }

        #endregion
        #region Public Methods

        public virtual void CompareResults()
        {
            StreamWriter comparisonResultsWriter = new StreamWriter(ComparisonFileLocation);
            StreamReader baseResultsReader = new StreamReader(BaseResultsFileLocation),
                newResultsReader = new StreamReader(NewResultsFileLocation);
            String baseResultsBuffer, newResultsBuffer;
            while ((baseResultsBuffer = baseResultsReader.ReadLine().Trim()).Equals("")) ;
            while ((newResultsBuffer = newResultsReader.ReadLine().Trim()).Equals("")) ;
            while (!baseResultsReader.EndOfStream)
            {
                String
                    baseResultsQueryType = baseResultsBuffer,
                    newResultsQueryType = newResultsBuffer,
                    baseResultsLine1 = baseResultsReader.ReadLine().Trim(),
                    newResultsLine1 = newResultsReader.ReadLine().Trim(),
                    baseResultsLine2 = baseResultsReader.ReadLine().Trim(),
                    newResultsLine2 = newResultsReader.ReadLine().Trim();

                if (!baseResultsQueryType.Equals(newResultsQueryType))
                    ReportQueryHeaderError(comparisonResultsWriter, baseResultsQueryType, newResultsQueryType, baseResultsLine1, newResultsLine1, baseResultsLine2, newResultsLine2, "Inconsistant Query Type", "");
                //else if (!baseResultsLine1.Equals(newResultsLine1))
                //    ReportQueryHeaderError(comparisonResultsWriter, baseResultsQueryType, newResultsQueryType, baseResultsLine1, newResultsLine1, baseResultsLine2, newResultsLine2, "Inconsistant Line 1", "");
                //else if (!baseResultsLine2.Equals(newResultsLine2))
                //    ReportQueryHeaderError(comparisonResultsWriter, baseResultsQueryType, newResultsQueryType, baseResultsLine1, newResultsLine1, baseResultsLine2, newResultsLine2, "Inconsistant Line 2", "");
                
                else
                {
                    List<Int32> baseResults = new List<Int32>(),
                        newResults = new List<Int32>();
                    baseResultsReader.ReadLine();
                    newResultsReader.ReadLine();
                    while (!(baseResultsBuffer = baseResultsReader.ReadLine().Trim()).Equals("Complementary Objects:"))
                        baseResults.Add(Int32.Parse(baseResultsBuffer.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]));
                    while (!(newResultsBuffer = newResultsReader.ReadLine().Trim()).Equals("Complementary Objects:"))
                        newResults.Add(Int32.Parse(newResultsBuffer.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]));
                    if (baseResultsQueryType.Equals("Traditional Range Search") || baseResultsQueryType.Equals("Traditional Window Search"))
                    {
                        List<Int32> resultsInBaseResultsButNotNewResults = new List<Int32>(),
                            resultsInNewResultsButNotBaseResults = new List<Int32>();
                        foreach (Int32 result in baseResults)
                            if (!newResults.Contains(result))
                                resultsInBaseResultsButNotNewResults.Add(result);
                        foreach (Int32 result in newResults)
                            if (!baseResults.Contains(result))
                                resultsInNewResultsButNotBaseResults.Add(result);
                        if (resultsInBaseResultsButNotNewResults.Count > 0 || resultsInNewResultsButNotBaseResults.Count > 0)
                        {
                            String conflictingResults = "Results in Base output that are not in New output:" + Environment.NewLine;
                            foreach (Int32 result in resultsInBaseResultsButNotNewResults)
                                conflictingResults += result.ToString() + Environment.NewLine;
                            conflictingResults += "Results in New output that are not in Base output:" + Environment.NewLine;
                            foreach (Int32 result in resultsInNewResultsButNotBaseResults)
                                conflictingResults += result.ToString() + Environment.NewLine;
                            ReportQueryHeaderError(comparisonResultsWriter, baseResultsQueryType, newResultsQueryType, baseResultsLine1, newResultsLine1, baseResultsLine2, newResultsLine2, "Inconsistant Query Results", conflictingResults);
                        }
                    }
                    else if (baseResultsQueryType.Equals("Traditional Nearest Neighbor Search"))
                    {
                        for (int i = 0; i < baseResults.Count - 1; i++)
                            if (i >= newResults.Count || baseResults[i] != newResults[i])
                            {
                                String conflictingResults = "Results in Base output:" + Environment.NewLine;
                                foreach (Int32 result in baseResults)
                                    conflictingResults += result.ToString() + Environment.NewLine;
                                conflictingResults += "Results in New output:" + Environment.NewLine;
                                foreach (Int32 result in newResults)
                                    conflictingResults += result.ToString() + Environment.NewLine;
                                ReportQueryHeaderError(comparisonResultsWriter, baseResultsQueryType, newResultsQueryType, baseResultsLine1, newResultsLine1, baseResultsLine2, newResultsLine2, "Inconsistant Query Results", conflictingResults);
                                break;
                            }

                    }
                    else
                        throw new Exception();
                }
                while (!baseResultsReader.EndOfStream && !(baseResultsBuffer = baseResultsReader.ReadLine().Trim()).Equals("")) ;
                while (!newResultsReader.EndOfStream && !(newResultsBuffer = newResultsReader.ReadLine().Trim()).Equals("")) ;
                if (!baseResultsReader.EndOfStream)
                    baseResultsBuffer = baseResultsReader.ReadLine().Trim();
                if (!newResultsReader.EndOfStream)
                    newResultsBuffer = newResultsReader.ReadLine().Trim();
            }
            baseResultsReader.Close();
            comparisonResultsWriter.Close();
            newResultsReader.Close();
        }

        #endregion
        #region Protected Methods

        protected virtual void ReportQueryHeaderError(StreamWriter comparisonResultsWriter,
            String baseResultsQueryType, String newResultsQueryType, String baseResultsLine1,
            String newResultsLine1, String baseResultsLine2, String newResultsLine2,
            String errorType, String other)
        {
            comparisonResultsWriter.WriteLine(errorType);
            comparisonResultsWriter.WriteLine(baseResultsQueryType);
            comparisonResultsWriter.WriteLine(baseResultsLine1);
            comparisonResultsWriter.WriteLine(baseResultsLine2);

            comparisonResultsWriter.WriteLine(newResultsQueryType);
            comparisonResultsWriter.WriteLine(newResultsLine1);
            comparisonResultsWriter.WriteLine(newResultsLine2);
            comparisonResultsWriter.WriteLine(other);
            comparisonResultsWriter.WriteLine();
        }

        #endregion
    }
}
