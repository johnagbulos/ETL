using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingsETL
{
    public class StoredProcedureHandler
    {
        private Azure azure;

        public StoredProcedureHandler()
        {
            azure = new Azure();
        }

        public void TriggerProcedures()
        {
            foreach (string s in Globals.StoredProcedures)
                azure.ExecuteStoredProcedure(s);
        }
    }
}
