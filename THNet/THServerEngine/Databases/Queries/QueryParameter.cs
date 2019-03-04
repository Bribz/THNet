using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THServerEngine.Databases
{
    public class QueryParameter
    {
        public string name;
        public Type valueType;
        public object value;

        public QueryParameter(string _name, Type _valueType, object _value)
        {
            name = _name;
            valueType = _valueType;
            value = _value;
        }
    }
}
