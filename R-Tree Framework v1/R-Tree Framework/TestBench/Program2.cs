using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Edu.Psu.Cse.R_Tree_Framework.Framework;
using Edu.Psu.Cse.R_Tree_Framework.Indexes;
using Edu.Psu.Cse.R_Tree_Framework.CacheManagers;
using System.IO;

namespace TestBench
{
    class Program2
    {
        internal static void Main(string[] args)
        {
            String loc = @"C:\Users\Mike\Documents\R-Tree Framework\trunk\Experiments\TEMP\";
            foreach (FileInfo file in (new DirectoryInfo(loc)).GetFiles())
            {
                StreamReader resultReader = new StreamReader(file.FullName);
                StreamWriter statWriter = new StreamWriter(file.FullName + ".s");
                while (!resultReader.EndOfStream)
                {
                    while (!resultReader.ReadLine().Equals("Statistics")) ;
                    String[] buffer = resultReader.ReadLine().Split('\t');
                    statWriter.WriteLine("{0},{1},{2},{3}", buffer[1], buffer[3], buffer[5], buffer[7]);
                }
                resultReader.Close();
                statWriter.Close();
            }






            /*String bob = @"C:\Users\Mike\Documents\R-Tree Framework\trunk\Experiments\Cache";
            foreach (FileInfo file in (new DirectoryInfo(bob)).GetFiles())
            {
                    StreamReader cacheReader = new StreamReader(file.FullName);
                    String memloc = cacheReader.ReadLine();
                    FileStream memoryReader = new FileStream(
                    memloc,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.None,
                    8,
                    FileOptions.WriteThrough | FileOptions.RandomAccess);
                    Byte[] dataBlock = new Byte[Constants.PAGE_SIZE];
                    Int32 address = 0;
                    Int32 numberOfNodes = 0;
                    while (address < memoryReader.Length)
                    {
                        memoryReader.Seek(address - address % Constants.PAGE_SIZE, SeekOrigin.Begin);
                        memoryReader.Read(dataBlock, 0, Constants.PAGE_SIZE);
                        address += Constants.PAGE_SIZE;
                        if (dataBlock[0] == (Byte)PageDataType.Leaf || dataBlock[0] == (Byte)PageDataType.Node)
                            numberOfNodes++;
                    }
                    memoryReader.Close();
                    StreamWriter cachewriter = new StreamWriter(file.FullName + "xx");
                    cachewriter.WriteLine(memloc);
                    cachewriter.WriteLine(cacheReader.ReadLine());
                    cachewriter.WriteLine(cacheReader.ReadLine());
                    cachewriter.WriteLine(numberOfNodes);
                    while(!cacheReader.EndOfStream)
                        cachewriter.WriteLine(cacheReader.ReadLine());
                    cacheReader.Close();
                    cachewriter.Close();
                    File.Delete(file.FullName);
                    File.Move(file.FullName + "xx", file.FullName);
            }
           // (new Program2()).Run();*/
        }
        /*public void Run()
        {
            String build = "Release";

            String rootFileLocation = @"C:\Users\Mike\Documents\R-Tree Framework\trunk\Experiments\",
                db = @"Databases\",
                ds = @"DataSets\",
                i = @"Indexes\",
                c = @"Caches\",
                m = @"Memories\",
                r = @"Results\",
                b = @"BaseResults\",
                cr = @"ComparisonResults\",
                u_s = @"uniform_small",
                u_l = @"uniform_large",
                r_s = @"real_small",
                r_l = @"real_large",
                indexBuilderLocation = @"C:\Users\Mike\Documents\R-Tree Framework\trunk\R-Tree Framework\IndexBuilder\bin\" + build + @"\IndexBuilder.exe",
                queryPlanExecutorFileLocation = @"C:\Users\Mike\Documents\R-Tree Framework\trunk\R-Tree Framework\QueryPlanExecutor\bin\" + build + @"\QueryPlanExecutor.exe",
                resultComparitorFileLocation = @"C:\Users\Mike\Documents\R-Tree Framework\trunk\R-Tree Framework\ResultComparitor\bin\" + build +  @"\ResultComparitor.exe";
            
            Int32 numberOfExperiments = 16;

            Type cacheType = typeof(LRUCacheManager);
            Int32
                pageSize = Constants.PAGE_SIZE,
                cacheSize = 0,
                //maximumNodeOcupancy = Constants.NODE_ENTRIES_PER_NODE,
                //minimumNodeOccupancy = maximumNodeOcupancy * 3 / 10;
            String queryPlanFileLocation = rootFileLocation + @"QueryPlans\large_q30.dat",
                ext;
            Type[]
                treeType = new Type[numberOfExperiments];
            String[]
                databaseFileLocation = new String[numberOfExperiments],
                dataSetFileLocation = new String[numberOfExperiments],
                indexSaveFileLocation = new String[numberOfExperiments],
                cacheSaveFileLocation = new String[numberOfExperiments],
                memorySaveFileLocation = new String[numberOfExperiments],
                resultsSaveFileLocation = new String[numberOfExperiments],
                baseResultsFileLocation = new String[numberOfExperiments],
                comparisonResultsFileLocation = new String[numberOfExperiments];

            #region R-Tree Uniform Small
            treeType[0] = typeof(R_Tree);
            ext = ".r_tree";
            databaseFileLocation[0] = rootFileLocation + db + u_s + ext;
            dataSetFileLocation[0] = rootFileLocation + ds + u_s + ".dat";
            indexSaveFileLocation[0] = rootFileLocation + i + u_s + ext;
            cacheSaveFileLocation[0] = rootFileLocation + c + u_s + ext;
            memorySaveFileLocation[0] = rootFileLocation + m + u_s + ext;
            resultsSaveFileLocation[0] = rootFileLocation + r + u_s + ext;
            baseResultsFileLocation[0] = rootFileLocation + b + u_s + "_q30_mod_out.txt";
            comparisonResultsFileLocation[0] = rootFileLocation + cr + u_s + ext;
            #endregion
            #region R-Tree Uniform Large
            treeType[1] = typeof(R_Tree);
            ext = ".r_tree";
            databaseFileLocation[1] = rootFileLocation + db + u_l + ext;
            dataSetFileLocation[1] = rootFileLocation + ds + u_l + ".dat";
            indexSaveFileLocation[1] = rootFileLocation + i + u_l + ext;
            cacheSaveFileLocation[1] = rootFileLocation + c + u_l + ext;
            memorySaveFileLocation[1] = rootFileLocation + m + u_l + ext;
            resultsSaveFileLocation[1] = rootFileLocation + r + u_l + ext;
            baseResultsFileLocation[1] = rootFileLocation + b + u_l + "_q30_mod_out.txt";
            comparisonResultsFileLocation[1] = rootFileLocation + cr + u_l + ext;
            #endregion
            #region R-Tree Real Small
            treeType[2] = typeof(R_Tree);
            ext = ".r_tree";
            databaseFileLocation[2] = rootFileLocation + db + r_s + ext;
            dataSetFileLocation[2] = rootFileLocation + ds + r_s + ".dat";
            indexSaveFileLocation[2] = rootFileLocation + i + r_s + ext;
            cacheSaveFileLocation[2] = rootFileLocation + c + r_s + ext;
            memorySaveFileLocation[2] = rootFileLocation + m + r_s + ext;
            resultsSaveFileLocation[2] = rootFileLocation + r + r_s + ext;
            baseResultsFileLocation[2] = rootFileLocation + b + r_s + "_q30_mod_out.txt";
            comparisonResultsFileLocation[2] = rootFileLocation + cr + r_s + ext;
            #endregion
            #region R-Tree Real Large
            treeType[3] = typeof(R_Tree);
            ext = ".r_tree";
            databaseFileLocation[3] = rootFileLocation + db + r_l + ext;
            dataSetFileLocation[3] = rootFileLocation + ds + r_l + ".dat";
            indexSaveFileLocation[3] = rootFileLocation + i + r_l + ext;
            cacheSaveFileLocation[3] = rootFileLocation + c + r_l + ext;
            memorySaveFileLocation[3] = rootFileLocation + m + r_l + ext;
            resultsSaveFileLocation[3] = rootFileLocation + r + r_l + ext;
            baseResultsFileLocation[3] = rootFileLocation + b + r_l + "_q30_mod_out.txt";
            comparisonResultsFileLocation[3] = rootFileLocation + cr + r_l + ext;
            #endregion
            #region R*-Tree Uniform Small
            treeType[4] = typeof(R_Star_Tree);
            ext = ".r_star_tree";
            databaseFileLocation[4] = rootFileLocation + db + u_s + ext;
            dataSetFileLocation[4] = rootFileLocation + ds + u_s + ".dat";
            indexSaveFileLocation[4] = rootFileLocation + i + u_s + ext;
            cacheSaveFileLocation[4] = rootFileLocation + c + u_s + ext;
            memorySaveFileLocation[4] = rootFileLocation + m + u_s + ext;
            resultsSaveFileLocation[4] = rootFileLocation + r + u_s + ext;
            baseResultsFileLocation[4] = rootFileLocation + b + u_s + "_q30_mod_out.txt";
            comparisonResultsFileLocation[4] = rootFileLocation + cr + u_s + ext;
            #endregion
            #region R*-Tree Uniform Large
            treeType[5] = typeof(R_Star_Tree);
            ext = ".r_star_tree";
            databaseFileLocation[5] = rootFileLocation + db + u_l + ext;
            dataSetFileLocation[5] = rootFileLocation + ds + u_l + ".dat";
            indexSaveFileLocation[5] = rootFileLocation + i + u_l + ext;
            cacheSaveFileLocation[5] = rootFileLocation + c + u_l + ext;
            memorySaveFileLocation[5] = rootFileLocation + m + u_l + ext;
            resultsSaveFileLocation[5] = rootFileLocation + r + u_l + ext;
            baseResultsFileLocation[5] = rootFileLocation + b + u_l + "_q30_mod_out.txt";
            comparisonResultsFileLocation[5] = rootFileLocation + cr + u_l + ext;
            #endregion
            #region R*-Tree Real Small
            treeType[6] = typeof(R_Star_Tree);
            ext = ".r_star_tree";
            databaseFileLocation[6] = rootFileLocation + db + r_s + ext;
            dataSetFileLocation[6] = rootFileLocation + ds + r_s + ".dat";
            indexSaveFileLocation[6] = rootFileLocation + i + r_s + ext;
            cacheSaveFileLocation[6] = rootFileLocation + c + r_s + ext;
            memorySaveFileLocation[6] = rootFileLocation + m + r_s + ext;
            resultsSaveFileLocation[6] = rootFileLocation + r + r_s + ext;
            baseResultsFileLocation[6] = rootFileLocation + b + r_s + "_q30_mod_out.txt";
            comparisonResultsFileLocation[6] = rootFileLocation + cr + r_s + ext;
            #endregion
            #region R*-Tree Real Large
            treeType[7] = typeof(R_Star_Tree);
            ext = ".r_star_tree";
            databaseFileLocation[7] = rootFileLocation + db + r_l + ext;
            dataSetFileLocation[7] = rootFileLocation + ds + r_l + ".dat";
            indexSaveFileLocation[7] = rootFileLocation + i + r_l + ext;
            cacheSaveFileLocation[7] = rootFileLocation + c + r_l + ext;
            memorySaveFileLocation[7] = rootFileLocation + m + r_l + ext;
            resultsSaveFileLocation[7] = rootFileLocation + r + r_l + ext;
            baseResultsFileLocation[7] = rootFileLocation + b + r_l + "_q30_mod_out.txt";
            comparisonResultsFileLocation[7] = rootFileLocation + cr + r_l + ext;
            #endregion
            #region Flash R-Tree Uniform Small
            treeType[8] = typeof(Flash_R_Tree);
            ext = ".flash_r_tree";
            databaseFileLocation[8] = rootFileLocation + db + u_s + ext;
            dataSetFileLocation[8] = rootFileLocation + ds + u_s + ".dat";
            indexSaveFileLocation[8] = rootFileLocation + i + u_s + ext;
            cacheSaveFileLocation[8] = rootFileLocation + c + u_s + ext;
            memorySaveFileLocation[8] = rootFileLocation + m + u_s + ext;
            resultsSaveFileLocation[8] = rootFileLocation + r + u_s + ext;
            baseResultsFileLocation[8] = rootFileLocation + b + u_s + "_q30_mod_out.txt";
            comparisonResultsFileLocation[8] = rootFileLocation + cr + u_s + ext;
             #endregion
            #region Flash R-Tree Uniform Large
            treeType[9] = typeof(Flash_R_Tree);
            ext = ".flash_r_tree";
            databaseFileLocation[9] = rootFileLocation + db + u_l + ext;
            dataSetFileLocation[9] = rootFileLocation + ds + u_l + ".dat";
            indexSaveFileLocation[9] = rootFileLocation + i + u_l + ext;
            cacheSaveFileLocation[9] = rootFileLocation + c + u_l + ext;
            memorySaveFileLocation[9] = rootFileLocation + m + u_l + ext;
            resultsSaveFileLocation[9] = rootFileLocation + r + u_l + ext;
            baseResultsFileLocation[9] = rootFileLocation + b + u_l + "_q30_mod_out.txt";
            comparisonResultsFileLocation[9] = rootFileLocation + cr + u_l + ext;
            #endregion
            #region Flash R-Tree Real Small
            treeType[10] = typeof(Flash_R_Tree);
            ext = ".flash_r_tree";
            databaseFileLocation[10] = rootFileLocation + db + r_s + ext;
            dataSetFileLocation[10] = rootFileLocation + ds + r_s + ".dat";
            indexSaveFileLocation[10] = rootFileLocation + i + r_s + ext;
            cacheSaveFileLocation[10] = rootFileLocation + c + r_s + ext;
            memorySaveFileLocation[10] = rootFileLocation + m + r_s + ext;
            resultsSaveFileLocation[10] = rootFileLocation + r + r_s + ext;
            baseResultsFileLocation[10] = rootFileLocation + b + r_s + "_q30_mod_out.txt";
            comparisonResultsFileLocation[10] = rootFileLocation + cr + r_s + ext;
            #endregion
            #region Flash R-Tree Real Large
            treeType[11] = typeof(Flash_R_Tree);
            ext = ".flash_r_tree";
            databaseFileLocation[11] = rootFileLocation + db + r_l + ext;
            dataSetFileLocation[11] = rootFileLocation + ds + r_l + ".dat";
            indexSaveFileLocation[11] = rootFileLocation + i + r_l + ext;
            cacheSaveFileLocation[11] = rootFileLocation + c + r_l + ext;
            memorySaveFileLocation[11] = rootFileLocation + m + r_l + ext;
            resultsSaveFileLocation[11] = rootFileLocation + r + r_l + ext;
            baseResultsFileLocation[11] = rootFileLocation + b + r_l + "_q30_mod_out.txt";
            comparisonResultsFileLocation[11] = rootFileLocation + cr + r_l + ext;
            #endregion
            #region Flash R-Tree Extended Uniform Small
            treeType[12] = typeof(Flash_R_Tree_Extended);
            ext = ".flash_r_tree_extended";
            databaseFileLocation[12] = rootFileLocation + db + u_s + ext;
            dataSetFileLocation[12] = rootFileLocation + ds + u_s + ".dat";
            indexSaveFileLocation[12] = rootFileLocation + i + u_s + ext;
            cacheSaveFileLocation[12] = rootFileLocation + c + u_s + ext;
            memorySaveFileLocation[12] = rootFileLocation + m + u_s + ext;
            resultsSaveFileLocation[12] = rootFileLocation + r + u_s + ext;
            baseResultsFileLocation[12] = rootFileLocation + b + u_s + "_q30_mod_out.txt";
            comparisonResultsFileLocation[12] = rootFileLocation + cr + u_s + ext;
             #endregion
            #region Flash R-Tree Extended Uniform Large
            treeType[13] = typeof(Flash_R_Tree_Extended);
            ext = ".flash_r_tree_extended";
            databaseFileLocation[13] = rootFileLocation + db + u_l + ext;
            dataSetFileLocation[13] = rootFileLocation + ds + u_l + ".dat";
            indexSaveFileLocation[13] = rootFileLocation + i + u_l + ext;
            cacheSaveFileLocation[13] = rootFileLocation + c + u_l + ext;
            memorySaveFileLocation[13] = rootFileLocation + m + u_l + ext;
            resultsSaveFileLocation[13] = rootFileLocation + r + u_l + ext;
            baseResultsFileLocation[13] = rootFileLocation + b + u_l + "_q30_mod_out.txt";
            comparisonResultsFileLocation[13] = rootFileLocation + cr + u_l + ext;
            #endregion
            #region Flash R-Tree Extended Real Small
            treeType[14] = typeof(Flash_R_Tree_Extended);
            ext = ".flash_r_tree_extended";
            databaseFileLocation[14] = rootFileLocation + db + r_s + ext;
            dataSetFileLocation[14] = rootFileLocation + ds + r_s + ".dat";
            indexSaveFileLocation[14] = rootFileLocation + i + r_s + ext;
            cacheSaveFileLocation[14] = rootFileLocation + c + r_s + ext;
            memorySaveFileLocation[14] = rootFileLocation + m + r_s + ext;
            resultsSaveFileLocation[14] = rootFileLocation + r + r_s + ext;
            baseResultsFileLocation[14] = rootFileLocation + b + r_s + "_q30_mod_out.txt";
            comparisonResultsFileLocation[14] = rootFileLocation + cr + r_s + ext;
            #endregion
            #region Flash R-Tree Extended Real Large
            treeType[15] = typeof(Flash_R_Tree_Extended);
            ext = ".flash_r_tree_extended";
            databaseFileLocation[15] = rootFileLocation + db + r_l + ext;
            dataSetFileLocation[15] = rootFileLocation + ds + r_l + ".dat";
            indexSaveFileLocation[15] = rootFileLocation + i + r_l + ext;
            cacheSaveFileLocation[15] = rootFileLocation + c + r_l + ext;
            memorySaveFileLocation[15] = rootFileLocation + m + r_l + ext;
            resultsSaveFileLocation[15] = rootFileLocation + r + r_l + ext;
            baseResultsFileLocation[15] = rootFileLocation + b + r_l + "_q30_mod_out.txt";
            comparisonResultsFileLocation[15] = rootFileLocation + cr + r_l + ext;
            #endregion

            Process
                indexBuilder = new Process(),
                queryPlanExecutor = new Process(),
                resultComparitor = new Process();

            indexBuilder.StartInfo.FileName = indexBuilderLocation;
            queryPlanExecutor.StartInfo.FileName = queryPlanExecutorFileLocation;
            resultComparitor.StartInfo.FileName = resultComparitorFileLocation;
            for (int k = 0; k < 4 && k < numberOfExperiments; k++)
            {
                indexBuilder.StartInfo.Arguments =
                    String.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\" \"{7}\" \"{8}\" \"{9}\" \"{10}\"",
                    cacheType.AssemblyQualifiedName,
                    treeType[k].AssemblyQualifiedName,
                    databaseFileLocation[k],
                    dataSetFileLocation[k],
                    indexSaveFileLocation[k],
                    cacheSaveFileLocation[k],
                    memorySaveFileLocation[k],
                    pageSize.ToString(),
                    cacheSize.ToString(),
                    minimumNodeOccupancy.ToString(),
                    maximumNodeOcupancy.ToString());
                queryPlanExecutor.StartInfo.Arguments =
                    String.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\"",
                    queryPlanFileLocation,
                    resultsSaveFileLocation[k],
                    cacheSaveFileLocation[k],
                    indexSaveFileLocation[k],
                    cacheType.AssemblyQualifiedName,
                    treeType[k].AssemblyQualifiedName,
                    cacheSize.ToString());
                resultComparitor.StartInfo.Arguments =
                    String.Format("\"{0}\" \"{1}\" \"{2}\"",
                    baseResultsFileLocation[k],
                    resultsSaveFileLocation[k],
                    comparisonResultsFileLocation[k]);
                Console.WriteLine("Running Experiment {0}", k);
                Console.WriteLine("\tBuilding Index");
                indexBuilder.Start();
                indexBuilder.WaitForExit();
                Console.WriteLine("\tExecuting Query Plan");
                queryPlanExecutor.Start();
                queryPlanExecutor.WaitForExit();
                Console.WriteLine("\tComparing Results");
                resultComparitor.Start();
                resultComparitor.WaitForExit();
            }
        }*/
    }
}