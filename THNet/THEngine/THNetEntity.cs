using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THEngine
{
    public class THNetEntity
    {
        public uint networkID;

        public virtual void RPC(byte rpcID)
        {

        }
    }
}
