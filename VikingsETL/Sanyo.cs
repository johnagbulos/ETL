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
    public class Sanyo
    {
        private string connString;
        private SqlConnection conn { get; set; }

        public Sanyo()
        {
            string ServerName = @"10.10.5.52\sanyo";
            string Database = "POSMAGIC";
            string UserID = @"ccread";
            string Password = "fishes";

            connString = String.Format(@"Server=tcp:{0};Database={1};User ID={2};Password={3};connection timeout=600;",
                                            ServerName,
                                            Database,
                                            UserID,
                                            Password);

            try
            {
                conn = new SqlConnection(connString);
                conn.Open();
            }
            catch (Exception ex)
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

        public void PopulateData(ref List<string[]> output, string query)
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader r = cmd.ExecuteReader();
            while(r.Read())
            {
                string[] tempArr = new string[r.FieldCount];
                for (int i = 0; i < r.FieldCount; i++)
                    tempArr[i] = r[i].ToString().Trim();
                output.Add(tempArr);
            }
            r.Close();
        }
    }
}
