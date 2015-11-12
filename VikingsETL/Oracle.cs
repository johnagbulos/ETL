using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace VikingsETL
{
    public class Oracle
    {
        private string connString;
        private OracleConnection conn;

        public Oracle()
        {
            string User = "membcon";
            string Password = "membcon";
            string Server = @"10.10.20.10/S7S";

            try
            {
                conn = new OracleConnection();
                conn.ConnectionString = String.Format("User Id={0};Password = {1};Data Source={2}", User, Password, Server);
                conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("--- Error in Oracle class ---");
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
            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            cmd.Connection = conn;
            OracleDataReader r = cmd.ExecuteReader();
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
