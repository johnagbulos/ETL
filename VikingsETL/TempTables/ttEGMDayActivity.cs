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
    public class ttEGMDayActivity
    {
        public DatabaseType SourceType = DatabaseType.Sanyo;
        public string UpdateIdentifier = "GAMING_DATE";
        public string SourceName = "V_NSW_EGM_DAYACTIVITY";
        public string RealTableName = "tEGMDayActivity";
        public string DestinationTableName = "ttEGMDayActivity";
        public string ExportFile = String.Empty;
        public string ExportFormatFile = String.Empty;
        public List<string[]> SourceData = new List<string[]>();

        public ttEGMDayActivity()
        {
            ExportFile = System.IO.Directory.GetCurrentDirectory() + @"\" + SourceName + ".csv";
            ExportFormatFile = System.IO.Directory.GetCurrentDirectory() + @"\" + SourceName + ".fmt";
        }

        public void PopulateData()
        {
            //Get last field PK
            Azure azure = new Azure();
            //string queryAzure = "SELECT MAX(CONVERT(varchar, GamingDate, 6)) FROM " + RealTableName;
            string queryAzure = "SELECT MAX(GamingDate) FROM " + RealTableName;
            string maxID = azure.GetSingleValue(queryAzure);
            DateTime dateMax = Convert.ToDateTime(maxID);

            //Get source data
            Oracle oracle = new Oracle();
            string queryOracle = String.Format("SELECT * FROM {0} WHERE {1} > '{2}'", SourceName, UpdateIdentifier, dateMax.ToString("dd/MMM/yy"));
            oracle.PopulateData(ref SourceData, queryOracle);

            //Delete temp tables in destination
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
