using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace VikingsETL
{
    public class BCPHandler
    {
        public BCPHandler() { }

        public void Run(string DestinationName, string ExportFile, string ExportFormatFile)
        {
            Process p = new Process();
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "cmd.exe";
            start.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            start.Arguments = String.Format("/C bcp {0}.dbo.{1} in {2} -S tcp:{3} -U {4} -P {5} -f {6}",
                                                Globals.DestDatabase,
                                                DestinationName,
                                                ExportFile,
                                                Globals.DestServerName,
                                                Globals.DestUserID,
                                                Globals.DestPassword,
                                                ExportFormatFile
                                                );
            p.StartInfo = start;
            p.Start();
            p.WaitForExit();
        }
    }
}
