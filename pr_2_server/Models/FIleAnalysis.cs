using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pr_2_server.Models
{
    internal class FIleAnalysis
    {
        public string StoragePath { get; set; } = "received_files";
        public string ResultAnalysis { get; set; }
        public string AnalysisFile { get; set; }
        public string FileName { get; set; }
        public int lineCount { get; set; }
        public int wordCount { get; set; }
        public int charCount { get; set; }

        public string uniqueFileName { get; set; }

        public string filePath { get; set; }
        public string fileContent { get; set; }
    }
}
