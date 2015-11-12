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
    public class ttEGMPlayerSessions
    {
        public DatabaseType SourceType = DatabaseType.S7S;
        public string UpdateIdentifier = "EGMPLAYERSESSIONPK";
        public string SourceName = "V_EGMPLAYERSESSIONS";
        public string RealTableName = "tEGMPlayerSessions";
        public string DestinationTableName = "ttEGMPlayerSessions";
        public string ExportFile = String.Empty;
        public string ExportFormatFile = String.Empty;
        public List<string[]> SourceData = new List<string[]>();

        public ttEGMPlayerSessions()
        {
            ExportFile = System.IO.Directory.GetCurrentDirectory() + @"\" + SourceName + ".csv";
            ExportFormatFile = System.IO.Directory.GetCurrentDirectory() + @"\" + SourceName + ".fmt";
        }

        public void PopulateData()
        {
            //Get last field pk
            Azure azure = new Azure();
            string queryAzure = "SELECT MAX(PK) FROM " + RealTableName;
            string maxID = azure.GetSingleValue(queryAzure);

            //Get source data
            Oracle oracle = new Oracle();
            string queryOracle = String.Format("SELECT * FROM {0} WHERE {1} > {2}", SourceName, UpdateIdentifier, maxID);
            oracle.PopulateData(ref SourceData, queryOracle);

            //DELETE temp tables in destination
            azure.DeleteTempTable(DestinationTableName);
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
