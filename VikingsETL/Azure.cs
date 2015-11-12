using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace VikingsETL
{
    public class Azure
    {
        private string connString;
        private SqlConnection conn { get; set; }

        public Azure()
        {
            string ServerName = "fexv816b4j.database.windows.net";
            string Database = "Vikings";
            string UserID = @"ccdb@fexv816b4j.database.windows.net";
            string Password = "FishStick1";

            connString = String.Format("Server=tcp:{0},1433;Database={1};User ID={2};Password={3};Trusted_Connection=false;Encrypt=True;connection timeout=600;",
                                            ServerName,
                                            Database,
                                            UserID,
                                            Password);

            try
            {
                conn = new SqlConnection(connString);
                conn.Open();
            }
            catch(Exception ex)
            {
                Console.WriteLine("--- Error in MSSQL Class ---");
                Console.WriteLine(ex.Message);
                Console.WriteLine("-----");
#if DEBUG   
                throw ex;
#endif
            }
        }

        public ConnectionState ConnectionState()
        {
            return conn.State;
        }

        public string GetSingleValue(string query)
        {
            //ONLY USE FOR VALUE RETURNS
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandTimeout = 600;
            SqlDataReader r = cmd.ExecuteReader();
            string returnString = String.Empty;
            while(r.Read())
            {
                returnString = r[0].ToString().Trim();
            }
            r.Close();
            return returnString;
        }

        public void DeleteTempTable(string TableName)
        {
            string sql = "DELETE FROM " + TableName;
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandTimeout = 600;
            cmd.ExecuteNonQuery();
        }

        public void ExecuteStoredProcedure(string SPName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = SPName;
            cmd.CommandTimeout = 600;
            cmd.ExecuteNonQuery();
        }
    }
}
