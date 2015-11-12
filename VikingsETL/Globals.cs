using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace VikingsETL
{
    public static class Globals
    {
        public const string DestServerName = "fexv816b4j.database.windows.net";
        public const string DestDatabase = "Vikings";
        public const string DestUserID = @"ccdb@fexv816b4j.database.windows.net";
        public const string DestPassword = "FishStick1";

        public static List<string> StoredProcedures = new List<string>
        {
            "spEGMActivityMeters",
            "spEGMDayActivity",
            "spEGMGameSessionSummary",
            "spEGMLocations",
            "spEGMPlayerSessions",
            "spLocation",
            "spMembers",
            "spMemberTier",
            "spPlayerSession",
            "spPurchase",
            "spPurchaseDetails",
            "spTerminal"
        };
    }
}
