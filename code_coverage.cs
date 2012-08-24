using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.CodeCoverage;

namespace codecoverage2xml
{
    class Program
    {
        static void Main(string[] args)
        {
            // Process Command Line arguments
            // No Args means we go boom
            if (args.Length == 0) {
                Console.WriteLine("Usage is codecoverage.exe $pathtocodecoverage [$optionalxmloutputfilename]");
                return;
            }

            // We must end up with a code coverage file, and an xml file to write
            string codecoveragefile = "";
            string xmloutputfile = "";

            // populate the variables from the arguments
            // two variables, one is codecoveragefile, one is xmloutputfile
            if (args.Length > 1) {
                codecoveragefile = args[0];
                xmloutputfile = args[1];
            } else {
                codecoveragefile = args[0];
                xmloutputfile = Regex.Replace(codecoveragefile, "coverage$", "xml");
            }

            // bail if coverage file does not exist
            if (File.Exists(codecoveragefile)) {
                Console.WriteLine("Good coverage file, continue");
            } else {
                Console.WriteLine("{0} Does not exist", codecoveragefile);
                return;
            }

            // Create a coverage info object from the file
            CoverageInfoManager.SymPath = codecoveragefile;

            CoverageInfoManager.ExePath = codecoveragefile;

            CoverageInfo ci = CoverageInfoManager.CreateInfoFromFile(codecoveragefile);
            // Ask for the DataSet
            // The parameter must be null
            CoverageDS data = ci.BuildDataSet(null);

            // Write to XML
            data.WriteXml(xmloutputfile);
        }
    }
}