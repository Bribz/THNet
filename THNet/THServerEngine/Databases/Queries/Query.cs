using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THServerEngine.Databases
{
    public abstract class Query
    {
        public QueryParameter[] paramList;
        public string str;

        public bool isParameterized
        {
            get
            {
                return paramList != null && paramList.Length > 0;
            }
        }

        public Query()
        {
            str = "";
            paramList = null;
        }

        public virtual void Create(string request, params QueryParameter[] parameters)
        {
            str = request;
            if(parameters != null && parameters.Length > 0)
            {
                paramList = parameters;
            }
        }
    }
}
