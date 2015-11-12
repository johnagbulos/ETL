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
    public class ttPlayerSession
    {
        public DatabaseType SourceType = DatabaseType.Sanyo;
        public string UpdateIdentifier = "EGMPLAYERSESSIONPK";
        public string SourceName = "V_EGMPLAYERSESSION_DAILY";
        public string RealTableName = "tPlayerSession";
        public string DestinationTableName = "ttPlayerSession";
        public string ExportFile = String.Empty;
        public string ExportFormatFile = String.Empty;
        public List<string[]> SourceData = new List<string[]>();

        public ttPlayerSession()
        {
            ExportFile = System.IO.Directory.GetCurrentDirectory() + @"\" + SourceName + ".csv";
            ExportFormatFile = System.IO.Directory.GetCurrentDirectory() + @"\" + SourceName + ".fmt";
        }

        public void PopulateData()
        {
            //Get last field pk
            Azure azure = new Azure();
            string queryAzure = "SELECT MAX() FROM " + RealTableName;
            string maxID = azure.GetSingleValue(queryAzure);

            //Get source data

            //Delete temp tables in destination
        }

        public void ExportCSV()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string[] d in SourceData)
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
