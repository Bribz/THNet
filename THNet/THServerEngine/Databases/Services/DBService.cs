using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THServerEngine.Databases;

namespace THServerEngine.Databases
{
    public abstract class DBService
    {
        public DBService()
        {

        }

        public abstract QueryResponse Query(Query request);

        public abstract void QueryAsync(Query request, Action<QueryResponse> callback);
    }
}
