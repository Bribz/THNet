using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THServerEngine.Databases
{
    public class File_DBService : DBService
    {
        private string dataPath;

        public File_DBService(string _dataPath)
        {
            dataPath = _dataPath;
            if(!System.IO.File.Exists(_dataPath))
            {
                System.IO.File.Create(_dataPath);
            }
        }

        private void Write(string obj_name, string data)
        {
            string[] lines = System.IO.File.ReadAllLines(dataPath);
            for(int i = 0; i < lines.Length; i++)
            {
                string[] lineContents = lines[i].Split(new char[] { ' ' });
                if (lineContents[0].Contains(obj_name))
                {
                    lines[i] = $"{obj_name} {data}";
                    break;
                }
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(dataPath))
            {
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
                file.WriteLine($"{obj_name} {data}");
            }
        }

        private string Read(string obj_name)
        {
            string[] lines = System.IO.File.ReadAllLines(dataPath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] lineContents = lines[i].Split(new char[] { ' ' });
                if (lineContents[0].Contains(obj_name))
                {
                    return lineContents[1];
                }
            }

            return "";
        }

        /// <summary>
        /// Query an existing file. Reads or write contents based on query string. Example: "read dat_obj_2", "write dat_obj_2 fs34dsas6s8g98"
        /// </summary>
        /// <param name="request">Query to db</param>
        /// <returns></returns>
        public override QueryResponse Query(Query request)
        {
            QueryResponse response = null;

            string[] cmds = request.str.Split(new char[] { ' ' });

            switch(cmds[0].ToLower())
            {
                case "w":
                case "write":
                    {
                        Write(cmds[1], cmds[2]);
                        break;
                    }
                case "r":
                case "read":
                    {
                        string retval = Read(cmds[1]);
                        if (retval.Length > 0)
                        {
                            response = new QueryResponse();
                            response.data = Encoding.UTF8.GetBytes(Read(cmds[1]));
                        }
                        break;
                    }
            }

            return response;
        }

        public override void QueryAsync(Query request, Action<QueryResponse> callback)
        {
            Log.Write("QueryAsync not implemented!", LogType.SYSTEM);
        }
    }
}
