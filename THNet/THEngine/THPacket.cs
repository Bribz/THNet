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
    
    public class THPacket
    {
        public PacketType Type = PacketType.NULL;

        public virtual byte[] Serialize()
        {
            return null;
        }

        public virtual void Deserialize(byte[] data)
        {

        }
    }
    
    public class StringUpdatePacket : THPacket
    {
        public uint networkID;
        public byte stringID;
        public string value;

        public static StringUpdatePacket Create(uint netID, byte strID, string val)
        {
            StringUpdatePacket retVal = new StringUpdatePacket();
            retVal.Type = PacketType.StringUpdate;

            retVal.networkID = netID;
            retVal.stringID = strID;
            retVal.value = val;

            return retVal;
        }

        public override byte[] Serialize()
        {
            return null;
        }

        public override void Deserialize(byte[] data)
        {

        }
    }

    public class RPCPacket : THPacket
    {
        public uint networkID;
        public byte RPC_ID;
        public byte[] data;

        public static RPCPacket Create(uint netID, byte rpcID, byte[] paramData)
        {
            RPCPacket retVal = new RPCPacket();
            retVal.Type = PacketType.RPC;

            retVal.networkID = netID;
            retVal.RPC_ID = rpcID;
            retVal.data = paramData;

            return retVal;
        }

        public override byte[] Serialize()
        {
            return null;
        }

        public override void Deserialize(byte[] data)
        {

        }
    }
}
