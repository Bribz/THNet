using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;

namespace THEngine
{
    public enum PacketType : byte
    {
        ConnectionInformation = 0x0,
        NetworkObjectCreation = 0x1,
        NetworkObjectUpdate = 0x2,
        RPC = 0x3,

        StringUpdate = 0xE,

        Login = 0xF,
        LoginAccepted = 0x11,
        LoginDeclined = 0x12,

        NULL = 0xFF
    }
    
    [Union(typeof(StringUpdatePacket), typeof(RPCPacket))]
    public abstract class THPacket
    {
        [UnionKey]
        public abstract PacketType Type { get; }
    }

    [ZeroFormattable]
    public class StringUpdatePacket : THPacket
    {
        public override PacketType Type
        {
            get
            {
                return PacketType.StringUpdate;
            }
        }
        [Index(0)]
        public virtual uint networkID { get; set; }
        [Index(1)]
        public virtual byte stringID { get; set; }
        [Index(2)]
        public virtual string value { get; set; }

        public static StringUpdatePacket Create(uint netID, byte strID, string val)
        {
            StringUpdatePacket retVal = new StringUpdatePacket();

            retVal.networkID = netID;
            retVal.stringID = strID;
            retVal.value = val;

            return retVal;
        }
    }

    public class RPCPacket : THPacket
    {
        public override PacketType Type
        {
            get
            {
                return PacketType.RPC;
            }
        }

        [Index(0)]
        public virtual uint networkID { get; set; }
        [Index(1)]
        public virtual byte RPC_ID { get; set; }
        [Index(2)]
        public virtual byte[] data { get; set; }

        public static RPCPacket Create(uint netID, byte rpcID, byte[] paramData)
        {
            RPCPacket retVal = new RPCPacket();

            retVal.networkID = netID;
            retVal.RPC_ID = rpcID;
            retVal.data = paramData;

            return retVal;
        }
    }
}
