#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;
    using global::ZeroFormatter.Comparers;

    public static partial class ZeroFormatterInitializer
    {
        static bool registered = false;

        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Register()
        {
            if(registered) return;
            registered = true;
            // Enums
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::THEngine.PacketType>.Register(new ZeroFormatter.DynamicObjectSegments.THEngine.PacketTypeFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Comparers.ZeroFormatterEqualityComparer<global::THEngine.PacketType>.Register(new ZeroFormatter.DynamicObjectSegments.THEngine.PacketTypeEqualityComparer());
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::THEngine.PacketType?>.Register(new ZeroFormatter.DynamicObjectSegments.THEngine.NullablePacketTypeFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            ZeroFormatter.Comparers.ZeroFormatterEqualityComparer<global::THEngine.PacketType?>.Register(new NullableEqualityComparer<global::THEngine.PacketType>());
            
            // Objects
            ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::THEngine.StringUpdatePacket>.Register(new ZeroFormatter.DynamicObjectSegments.THEngine.StringUpdatePacketFormatter<ZeroFormatter.Formatters.DefaultResolver>());
            // Structs
            // Unions
            {
                var unionFormatter = new ZeroFormatter.DynamicObjectSegments.THEngine.THPacketFormatter<ZeroFormatter.Formatters.DefaultResolver>();
                ZeroFormatter.Formatters.Formatter<ZeroFormatter.Formatters.DefaultResolver, global::THEngine.THPacket>.Register(unionFormatter);
            }
            // Generics
        }
    }
}
#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments.THEngine
{
    using global::System;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;

    public class StringUpdatePacketFormatter<TTypeResolver> : Formatter<TTypeResolver, global::THEngine.StringUpdatePacket>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::THEngine.StringUpdatePacket value)
        {
            var segment = value as IZeroFormatterSegment;
            if (segment != null)
            {
                return segment.Serialize(ref bytes, offset);
            }
            else if (value == null)
            {
                BinaryUtil.WriteInt32(ref bytes, offset, -1);
                return 4;
            }
            else
            {
                var startOffset = offset;

                offset += (8 + 4 * (2 + 1));
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, uint>(ref bytes, startOffset, offset, 0, value.networkID);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, byte>(ref bytes, startOffset, offset, 1, value.stringID);
                offset += ObjectSegmentHelper.SerializeFromFormatter<TTypeResolver, string>(ref bytes, startOffset, offset, 2, value.value);

                return ObjectSegmentHelper.WriteSize(ref bytes, startOffset, offset, 2);
            }
        }

        public override global::THEngine.StringUpdatePacket Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = BinaryUtil.ReadInt32(ref bytes, offset);
            if (byteSize == -1)
            {
                byteSize = 4;
                return null;
            }
            return new StringUpdatePacketObjectSegment<TTypeResolver>(tracker, new ArraySegment<byte>(bytes, offset, byteSize));
        }
    }

    public class StringUpdatePacketObjectSegment<TTypeResolver> : global::THEngine.StringUpdatePacket, IZeroFormatterSegment
        where TTypeResolver : ITypeResolver, new()
    {
        static readonly int[] __elementSizes = new int[]{ 4, 1, 0 };

        readonly ArraySegment<byte> __originalBytes;
        readonly global::ZeroFormatter.DirtyTracker __tracker;
        readonly int __binaryLastIndex;
        readonly byte[] __extraFixedBytes;

        CacheSegment<TTypeResolver, string> _value;

        // 0
        public override uint networkID
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, uint>(__originalBytes, 0, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, uint>(__originalBytes, 0, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 1
        public override byte stringID
        {
            get
            {
                return ObjectSegmentHelper.GetFixedProperty<TTypeResolver, byte>(__originalBytes, 1, __binaryLastIndex, __extraFixedBytes, __tracker);
            }
            set
            {
                ObjectSegmentHelper.SetFixedProperty<TTypeResolver, byte>(__originalBytes, 1, __binaryLastIndex, __extraFixedBytes, value, __tracker);
            }
        }

        // 2
        public override string value
        {
            get
            {
                return _value.Value;
            }
            set
            {
                _value.Value = value;
            }
        }


        public StringUpdatePacketObjectSegment(global::ZeroFormatter.DirtyTracker dirtyTracker, ArraySegment<byte> originalBytes)
        {
            var __array = originalBytes.Array;

            this.__originalBytes = originalBytes;
            this.__tracker = dirtyTracker = dirtyTracker.CreateChild();
            this.__binaryLastIndex = BinaryUtil.ReadInt32(ref __array, originalBytes.Offset + 4);

            this.__extraFixedBytes = ObjectSegmentHelper.CreateExtraFixedBytes(this.__binaryLastIndex, 2, __elementSizes);

            _value = new CacheSegment<TTypeResolver, string>(__tracker, ObjectSegmentHelper.GetSegment(originalBytes, 2, __binaryLastIndex, __tracker));
        }

        public bool CanDirectCopy()
        {
            return !__tracker.IsDirty;
        }

        public ArraySegment<byte> GetBufferReference()
        {
            return __originalBytes;
        }

        public int Serialize(ref byte[] targetBytes, int offset)
        {
            if (__extraFixedBytes != null || __tracker.IsDirty)
            {
                var startOffset = offset;
                offset += (8 + 4 * (2 + 1));

                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, uint>(ref targetBytes, startOffset, offset, 0, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeFixedLength<TTypeResolver, byte>(ref targetBytes, startOffset, offset, 1, __binaryLastIndex, __originalBytes, __extraFixedBytes, __tracker);
                offset += ObjectSegmentHelper.SerializeCacheSegment<TTypeResolver, string>(ref targetBytes, startOffset, offset, 2, ref _value);

                return ObjectSegmentHelper.WriteSize(ref targetBytes, startOffset, offset, 2);
            }
            else
            {
                return ObjectSegmentHelper.DirectCopyAll(__originalBytes, ref targetBytes, offset);
            }
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments.THEngine
{
    using global::System;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;

    public class THPacketFormatter<TTypeResolver> : Formatter<TTypeResolver, global::THEngine.THPacket>
        where TTypeResolver : ITypeResolver, new()
    {
        readonly global::System.Collections.Generic.IEqualityComparer<global::THEngine.PacketType> comparer;
        readonly global::THEngine.PacketType[] unionKeys;
        
        public THPacketFormatter()
        {
            comparer = global::ZeroFormatter.Comparers.ZeroFormatterEqualityComparer<global::THEngine.PacketType>.Default;
            unionKeys = new global::THEngine.PacketType[2];
            unionKeys[0] = new global::THEngine.StringUpdatePacket().Type;
            unionKeys[1] = new global::THEngine.RPCPacket().Type;
            
        }

        public override int? GetLength()
        {
            return null;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::THEngine.THPacket value)
        {
            if (value == null)
            {
                return BinaryUtil.WriteInt32(ref bytes, offset, -1);
            }

            var startOffset = offset;

            offset += 4;
            offset += Formatter<TTypeResolver, global::THEngine.PacketType>.Default.Serialize(ref bytes, offset, value.Type);

            if (value is global::THEngine.StringUpdatePacket)
            {
                offset += Formatter<TTypeResolver, global::THEngine.StringUpdatePacket>.Default.Serialize(ref bytes, offset, (global::THEngine.StringUpdatePacket)value);
            }
            else if (value is global::THEngine.RPCPacket)
            {
                offset += Formatter<TTypeResolver, global::THEngine.RPCPacket>.Default.Serialize(ref bytes, offset, (global::THEngine.RPCPacket)value);
            }
            
            else
            {
                throw new Exception("Unknown subtype of Union:" + value.GetType().FullName);
            }
        
            var writeSize = offset - startOffset;
            BinaryUtil.WriteInt32(ref bytes, startOffset, writeSize);
            return writeSize;
        }

        public override global::THEngine.THPacket Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            if ((byteSize = BinaryUtil.ReadInt32(ref bytes, offset)) == -1)
            {
                byteSize = 4;
                return null;
            }
        
            offset += 4;
            int size;
            var unionKey = Formatter<TTypeResolver, global::THEngine.PacketType>.Default.Deserialize(ref bytes, offset, tracker, out size);
            offset += size;

            global::THEngine.THPacket result;
            if (comparer.Equals(unionKey, unionKeys[0]))
            {
                result = Formatter<TTypeResolver, global::THEngine.StringUpdatePacket>.Default.Deserialize(ref bytes, offset, tracker, out size);
            }
            else if (comparer.Equals(unionKey, unionKeys[1]))
            {
                result = Formatter<TTypeResolver, global::THEngine.RPCPacket>.Default.Deserialize(ref bytes, offset, tracker, out size);
            }
            else
            {
                throw new Exception("Unknown unionKey type of Union: " + unionKey.ToString());
            }

            return result;
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
namespace ZeroFormatter.DynamicObjectSegments.THEngine
{
    using global::System;
    using global::System.Collections.Generic;
    using global::ZeroFormatter.Formatters;
    using global::ZeroFormatter.Internal;
    using global::ZeroFormatter.Segments;


    public class PacketTypeFormatter<TTypeResolver> : Formatter<TTypeResolver, global::THEngine.PacketType>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return 1;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::THEngine.PacketType value)
        {
            return BinaryUtil.WriteByte(ref bytes, offset, (Byte)value);
        }

        public override global::THEngine.PacketType Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 1;
            return (global::THEngine.PacketType)BinaryUtil.ReadByte(ref bytes, offset);
        }
    }


    public class NullablePacketTypeFormatter<TTypeResolver> : Formatter<TTypeResolver, global::THEngine.PacketType?>
        where TTypeResolver : ITypeResolver, new()
    {
        public override int? GetLength()
        {
            return 2;
        }

        public override int Serialize(ref byte[] bytes, int offset, global::THEngine.PacketType? value)
        {
            BinaryUtil.WriteBoolean(ref bytes, offset, value.HasValue);
            if (value.HasValue)
            {
                BinaryUtil.WriteByte(ref bytes, offset + 1, (Byte)value.Value);
            }
            else
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, offset + 2);
            }

            return 2;
        }

        public override global::THEngine.PacketType? Deserialize(ref byte[] bytes, int offset, global::ZeroFormatter.DirtyTracker tracker, out int byteSize)
        {
            byteSize = 2;
            var hasValue = BinaryUtil.ReadBoolean(ref bytes, offset);
            if (!hasValue) return null;

            return (global::THEngine.PacketType)BinaryUtil.ReadByte(ref bytes, offset + 1);
        }
    }



    public class PacketTypeEqualityComparer : IEqualityComparer<global::THEngine.PacketType>
    {
        public bool Equals(global::THEngine.PacketType x, global::THEngine.PacketType y)
        {
            return (Byte)x == (Byte)y;
        }

        public int GetHashCode(global::THEngine.PacketType x)
        {
            return (int)(Byte)x;
        }
    }



}
#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
