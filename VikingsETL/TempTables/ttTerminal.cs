using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace VikingsETL.TempTables
{
    public class ttTerminal
    {
        public DatabaseType SourceType = DatabaseType.Sanyo;
        public string SourceName = "TerminalTable";
        public string DestinationName = "ttTerminal";
        public string ExportFile = String.Empty;
        public string ExportFormatFile = String.Empty;
        public List<string[]> SourceData = new List<string[]>();

        public ttTerminal()
        {
            ExportFile = System.IO.Directory.GetCurrentDirectory() + @"\" + SourceName + ".csv";
            ExportFormatFile = System.IO.Directory.GetCurrentDirectory() + @"\" + SourceName + ".fmt";
        }

        public void PopulateData()
        {
            //Get source data
            Sanyo sanyo = new Sanyo();
            sanyo.PopulateData(ref SourceData, "SELECT * FROM " + SourceName);

            //Delete temp tables in destination
            Azure azure = new Azure();
            azure.DeleteTempTable(DestinationName);
        }

        public void ExportCSV()
        {
            
            StringBuilder sb = new StringBuilder();
            foreach(string[] d in SourceData)
            {
                for (int i = 0; i < d.Length; i++)
                    sb.Append(d[i] + ",");
                sb.Append("\r\n");
            }

            sb.Replace(",\r\n", "\r\n");

            StreamWriter sw = new StreamWriter(ExportFile);
            sw.Write(sb.ToString());
            sw.Close();
        }
    }
}
