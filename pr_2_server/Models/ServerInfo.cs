using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace pr_2_server.Models
{
    internal class ServerInfo
    {
        public string IP { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 5555;

        //public string StoragePath { get; set; } = "received_files";
        //public string AnalysisFile { get; set; }

        //public string ResultAnalysis { get; set; }
    }
}
