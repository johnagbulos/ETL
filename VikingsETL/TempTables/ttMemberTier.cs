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
    public class ttMemberTier
    {
        public DatabaseType SourceType = DatabaseType.S7S;
        public string SourceName = "V_NSW_MEMBER_TIER";
        public string RealTableName = "tMemberTier";
        public string DestinationTableName = "ttMemberTier";
        public string ExportFile = String.Empty;
        public string ExportFormatFile = String.Empty;
        public List<string[]> SourceData = new List<string[]>();

        public ttMemberTier()
        {
            ExportFile = System.IO.Directory.GetCurrentDirectory() + @"\" + SourceName + ".csv";
            ExportFormatFile = System.IO.Directory.GetCurrentDirectory() + @"\" + SourceName + ".fmt";
        }

        public void PopulateData()
        {
            //Get all details, process in azure
            Oracle oracle = new Oracle();
            string query = "SELECT * FROM " + SourceName;
            oracle.PopulateData(ref SourceData, query);

            //Delete temp table in destination
            Azure azure = new Azure();
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
