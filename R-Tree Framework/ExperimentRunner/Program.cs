using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace ExperimentRunner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main(String[] args)
        {
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());*/
            //(new Program()).LoadExperiments(args[0]);
        }
        /*
        Program()
        {
        }

        void LoadExperiments(String experimentFileLocation)
        {
            StreamReader experimentReader = new StreamReader(experimentFileLocation);
            while (!experimentReader.EndOfStream)
            {
                String[] experimentVariables = experimentReader.ReadLine().Trim().Split(new char[] { ' ', ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            }
        }*/
    }
}