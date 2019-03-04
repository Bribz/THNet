//Dont use this, causes compile issues.
#if false

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace THServerEngine.Databases
{
    public class PGSQL_DBService : DBService
    {
        private string connStr;

        public PGSQL_DBService(string connectionString)
        {
            connStr = connectionString;    
        }

        public override QueryResponse Query(Query request)
        {
            QueryResponse retVal = null;

            using (NpgsqlConnection conn = new NpgsqlConnection(connStr))
            {
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = request.str;
                if (request.isParameterized)
                {
                    foreach (var param in request.paramList)
                    {
                        object paramValue = param.value;
                        if(param.valueType == typeof(object))
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            using (var ms = new MemoryStream())
                            {
                                bf.Serialize(ms, param.value);
                                paramValue = ms.ToArray();
                            }
                        }

                        cmd.Parameters.Add(new NpgsqlParameter()
                        {
                            ParameterName = param.name,
                            Value = paramValue,
                            NpgsqlDbType = NPGSQL_QueryHelpers.GetParameterType(param.valueType)
                        });
                    }
                }

                NpgsqlDataReader dr = cmd.ExecuteReader();

                //TODO: Data is not read. Find a reasonable solution to reading arbitrary data.
            }

            return retVal;
        }

        public override void QueryAsync(Query request, Action<QueryResponse> callback)
        {
            Log.Write("QueryAsync not implemented!", LogType.SYSTEM);
        }
    }

    public static class NPGSQL_QueryHelpers
    {
        public static Dictionary<Type, NpgsqlDbType> npgsqltype = new Dictionary<Type, NpgsqlDbType>()
        {
            { typeof(bool), NpgsqlDbType.Boolean },
            { typeof(int), NpgsqlDbType.Integer },
            { typeof(float), NpgsqlDbType.Real },
            { typeof(double), NpgsqlDbType.Double },
            { typeof(uint), NpgsqlDbType.Oid },
            { typeof(long), NpgsqlDbType.Bigint },
            { typeof(short), NpgsqlDbType.Smallint },
            { typeof(string), NpgsqlDbType.Varchar },
            { typeof(IPAddress), NpgsqlDbType.Inet },
            { typeof(Enum), NpgsqlDbType.Enum },
            { typeof(DateTime), NpgsqlDbType.Timestamp },
            { typeof(TimeSpan), NpgsqlDbType.Time },
            { typeof(byte[]), NpgsqlDbType.Bytea },
            { typeof(object), NpgsqlDbType.Bytea }
        };

        public static NpgsqlDbType GetParameterType(Type t)
        {
            return NPGSQL_QueryHelpers.npgsqltype[t];
        }
    }
}

#endif