using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;
using ZeroFormatter.Formatters;

namespace THEngine
{
    public enum PacketType : byte
    {
        ConnectionInformation = 0x0,
        NetworkObjectCreation,
        NetworkObjectUpdate,
        RPC,

        StringUpdate,

        Login,
        LoginAccepted,
        LoginDeclined,

        NULL = 0xFF
    }
    
    public static class THPacket
    {
        public static void Serialize(IPacket packet, out byte[] output)
        {
            output = ZeroFormatterSerializer.Serialize(packet);
        }

        public static IPacket Deserialize(byte[] input)
        {
            return ZeroFormatterSerializer.Deserialize<IPacket>(input);
        }

        /*
        public static PacketType GetTypeFromByte(byte b)
        {
            return (PacketType)b;
        }
        */

        public static void AppendDynamicUnionResolver()
        {
            Formatter.AppendDynamicUnionResolver((unionType, resolver) =>
            {
                if (unionType == typeof(IPacket))
                {
                    resolver.RegisterUnionKeyType(typeof(byte));
                    resolver.RegisterSubType(key: (byte)PacketType.RPC, subType: typeof(RPCPacket));
                    resolver.RegisterSubType(key: (byte)PacketType.StringUpdate, subType: typeof(StringUpdatePacket));
                    resolver.RegisterSubType(key: (byte)PacketType.Login, subType: typeof(LoginPacket));
                    resolver.RegisterFallbackType(typeof(NullPacket));
                }
            });
        }
    }
    
    [DynamicUnion]
    public class IPacket
    {
        //[UnionKey]
        [IgnoreFormat]
        public virtual PacketType Type => PacketType.NULL;
    }

    public class NullPacket : IPacket
    {
        [IgnoreFormat]
        public virtual PacketType Type => PacketType.NULL;

        [Index(0)]
        public virtual byte[] netID{ get; set; }
    }

    
    [ZeroFormattable]
    public class LoginPacket : IPacket
    {
        [IgnoreFormat]
        public override PacketType Type => PacketType.Login;

        [Index(0)]
        public virtual uint networkID { get; set; }
        [Index(1)]
        public virtual string emailHash { get; set; }
        [Index(2)]
        public virtual string passwordHash { get; set; }

        public static LoginPacket Create(uint netID, string email, string pass)
        {
            LoginPacket retVal = new LoginPacket();

            retVal.networkID = netID;
            retVal.emailHash = email;
            retVal.passwordHash = pass;

            return retVal;
        }
    }

    [ZeroFormattable]
    public class StringUpdatePacket : IPacket
    {
        [IgnoreFormat]
        public override PacketType Type => PacketType.StringUpdate;

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

    [ZeroFormattable]
    public class RPCPacket : IPacket
    {
        [IgnoreFormat]
        public override PacketType Type => PacketType.RPC;

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
