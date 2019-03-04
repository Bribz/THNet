using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THServerEngine.Databases;

namespace THServerEngine.Managers
{
    public class DatabaseManager
    {
        private Dictionary<string, DBService> _databaseServices;

        public DatabaseManager()
        {
            _databaseServices = new Dictionary<string, DBService>();
        }

        public void BindDBService(string db_name, DBService dbService)
        {
            _databaseServices.Add(db_name, dbService);
        }
        
        public void UnbindDBService(string db_name)
        {
            if(!_databaseServices.ContainsKey(db_name))
            {
                Log.Write($"No database service of id {db_name} to unbind. Skipping.", LogType.WARNING);
                return;
            }

            _databaseServices.Remove(db_name);
        }

        public DBService GetDBService(string db_name)
        {
            if (!_databaseServices.ContainsKey(db_name))
            {
                Log.Write($"No database service of id {db_name} is currently bound to Database Manager.", LogType.ERROR);
                return null;
            }

            return _databaseServices[db_name];
        }
    }
}
