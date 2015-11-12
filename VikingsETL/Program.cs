using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace VikingsETL
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----------------------");
            Console.WriteLine("       Vikings ETL    ");
            Console.WriteLine("----------------------");

#if DEBUG
            Console.Write("\n\nRun connection tests? ");
            string runConnectionState = Console.ReadLine();
            if (runConnectionState == "Y" || runConnectionState == "y" || runConnectionState == "yes" || runConnectionState == "YES" || runConnectionState == "Yes")
            {
                TestSanyoConnection();
                TestAzureConnection();
                TestOracleConnection();
            }
#endif

            Console.WriteLine("Syncing databases...\n--\n");
            LoadTerminals();
            LoadPurchaseDetails();
            LoadPurchase();
            LoadMemberTier();
            LoadMembers();
            LoadLocation();
            LoadEGMPlayerSessions();
            LoadEGMLocations();
            LoadEGMGameSessionSummary();
            LoadEGMDayActivity();
            LoadEGMActivityMeters();

            Console.WriteLine("Executing stored procedures...");
            LoadRealTables();

            Console.WriteLine("Clearing CSV files...");
            ClearCSVFiles();

            Console.WriteLine("DONE");
            Console.ReadLine();
        }

        static void LoadRealTables()
        {
            StoredProcedureHandler sp = new StoredProcedureHandler();
            sp.TriggerProcedures();
        }

        static void ClearCSVFiles()
        {
            //Get all csv files and delete
            string[] allFiles = Directory.GetFiles(System.IO.Directory.GetCurrentDirectory());
            for(int i = 0; i < allFiles.Length; i++)
            {
                if (allFiles[i].EndsWith(".csv"))
                    File.Delete(allFiles[i]);
            }
        }

        static void TestAzureConnection()
        {
            Azure mssql = new Azure();
            if (mssql.ConnectionState() == ConnectionState.Open)
                Console.WriteLine("Azure Connection Successful");
            else
                Console.WriteLine("Azure Connection not successful = " + mssql.ConnectionState().ToString());
        }

        static void TestOracleConnection()
        {
            Oracle oracle = new Oracle();
            if(oracle.ConnectionState() == ConnectionState.Open)
                Console.WriteLine("Oracle Connection Successful");
            else
                Console.WriteLine("Oracle Connection not successful = " + oracle.ConnectionState().ToString());
        }

        static void TestSanyoConnection()
        {
            Sanyo sanyo = new Sanyo();
            if (sanyo.ConnectionState() == ConnectionState.Open)
                Console.WriteLine("Sanyo Connection Successful");
            else
                Console.WriteLine("Sanyo Connection not successful = " + sanyo.ConnectionState().ToString());
        }

        static void LoadTerminals()
        {
            Console.WriteLine("Loading terminals...");

            TempTables.ttTerminal terminal = new TempTables.ttTerminal();
            terminal.PopulateData();
            terminal.ExportCSV();

            BCPHandler bcp = new BCPHandler();
            bcp.Run(terminal.DestinationName, terminal.ExportFile, terminal.ExportFormatFile);
        }

        static void LoadPurchaseDetails()
        {
            Console.WriteLine("Loading Purchase Details...");

            TempTables.ttPurchaseDetails pd = new TempTables.ttPurchaseDetails();
            pd.PopulateData();
            pd.ExportCSV();

            BCPHandler bcp = new BCPHandler();
            bcp.Run(pd.DestinationTableName, pd.ExportFile, pd.ExportFormatFile);
        }

        static void LoadPurchase()
        {
            Console.WriteLine("Loading Purchase...");

            TempTables.ttPurchase p = new TempTables.ttPurchase();
            p.PopulateData();
            p.ExportCSV();

            BCPHandler bcp = new BCPHandler();
            bcp.Run(p.DestinationTableName, p.ExportFile, p.ExportFormatFile);
        }

        static void LoadPlayerSession()
        {
            //Deprecated
        }

        static void LoadMemberTier()
        {
            Console.WriteLine("Loading Member Tier...");

            TempTables.ttMemberTier mt = new TempTables.ttMemberTier();
            mt.PopulateData();
            mt.ExportCSV();

            BCPHandler bcp = new BCPHandler();
            bcp.Run(mt.DestinationTableName, mt.ExportFile, mt.ExportFormatFile);
        }

        static void LoadMembers()
        {
            Console.WriteLine("Loading Members...");

            TempTables.ttMembers m = new TempTables.ttMembers();
            m.PopulateData();
            m.ExportCSV();

            BCPHandler bcp = new BCPHandler();
            bcp.Run(m.DestinationTableName, m.ExportFile, m.ExportFormatFile);
        }

        static void LoadLocation()
        {
            Console.WriteLine("Loading Locations...");

            TempTables.ttLocation l = new TempTables.ttLocation();
            l.PopulateData();
            l.ExportCSV();

            BCPHandler bcp = new BCPHandler();
            bcp.Run(l.DestinationTableName, l.ExportFile, l.ExportFormatFile);
        }

        static void LoadEGMPlayerSessions()
        {
            Console.WriteLine("Loading EGM Player Sessions...");

            TempTables.ttEGMPlayerSessions ps = new TempTables.ttEGMPlayerSessions();
            ps.PopulateData();
            ps.ExportCSV();

            BCPHandler bcp = new BCPHandler();
            bcp.Run(ps.DestinationTableName, ps.ExportFile, ps.ExportFormatFile);
        }

        static void LoadEGMLocations()
        {
            Console.WriteLine("Loading EGM Locations...");

            TempTables.ttEGMLocations el = new TempTables.ttEGMLocations();
            el.PopulateData();
            el.ExportCSV();

            BCPHandler bcp = new BCPHandler();
            bcp.Run(el.DestinationTableName, el.ExportFile, el.ExportFormatFile);
        }

        static void LoadEGMGameSessionSummary()
        {
            Console.WriteLine("Loading EGM Game Session Summary...");

            TempTables.ttEGMGameSessionSummary eg = new TempTables.ttEGMGameSessionSummary();
            eg.PopulateData();
            eg.ExportCSV();

            BCPHandler bcp = new BCPHandler();
            bcp.Run(eg.DestinationTableName, eg.ExportFile, eg.ExportFormatFile);
        }

        static void LoadEGMDayActivity()
        {
            Console.WriteLine("Loading EGM Day Activity...");

            TempTables.ttEGMDayActivity ed = new TempTables.ttEGMDayActivity();
            ed.PopulateData();
            ed.ExportCSV();

            BCPHandler bcp = new BCPHandler();
            bcp.Run(ed.DestinationTableName, ed.ExportFile, ed.ExportFormatFile);
        }

        static void LoadEGMActivityMeters()
        {
            Console.WriteLine("Loading EGM Activity Meters...");

            TempTables.ttEGMActivityMeters ea = new TempTables.ttEGMActivityMeters();
            ea.PopulateData();
            ea.ExportCSV();
            Console.WriteLine(ea.SourceData.Count.ToString());

            BCPHandler bcp = new BCPHandler();
            bcp.Run(ea.DestinationTableName, ea.ExportFile, ea.ExportFormatFile);
        }
    }
}
