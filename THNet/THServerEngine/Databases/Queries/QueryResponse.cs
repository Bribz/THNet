using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THServerEngine.Databases
{
    public class QueryResponse
    {
        public byte[] data;

        public QueryResponse()
        {
            data = new byte[1] { 0 };
        }

        public QueryResponse(byte[] _data)
        {
            data = _data;
        }
    }
}
