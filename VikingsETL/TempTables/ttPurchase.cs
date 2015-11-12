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
    public class ttPurchase
    {
        public DatabaseType SourceType = DatabaseType.Sanyo;
        public string UpdateIdentifier = "Transaction_Number";
        public string SourceName = "EJTable";
        public string RealTableName = "tPurchase";
        public string DestinationTableName = "ttPurchase";
        public string ExportFile = String.Empty;
        public string ExportFormatFile = String.Empty;
        public List<string[]> SourceData = new List<string[]>();

        public ttPurchase()
        {
            ExportFile = System.IO.Directory.GetCurrentDirectory() + @"\" + SourceName + ".csv";
            ExportFormatFile = System.IO.Directory.GetCurrentDirectory() + @"\" + SourceName + ".fmt";
        }

        public void PopulateData()
        {
            //Get last field PK
            Azure azure = new Azure();
            string queryAzure = "SELECT MAX(TransactionNumber) FROM " + RealTableName;
            string maxID = azure.GetSingleValue(queryAzure);

            //Get source data
            Sanyo sanyo = new Sanyo();
            string querySanyo = String.Format("SELECT * FROM {0} WHERE {1} > {2}", SourceName, UpdateIdentifier, maxID);
            sanyo.PopulateData(ref SourceData, querySanyo);

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
