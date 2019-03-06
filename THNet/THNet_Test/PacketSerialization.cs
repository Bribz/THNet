using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using THEngine;
using ZeroFormatter;

namespace THNet_Test
{
    [TestClass]
    public class PacketSerialization
    {
        public const int ARRAY_TEST_COUNT = 50000;

        public void RegisterZeroFormatter()
        {
            ZeroFormatterInitializer.Register();
        }

        #region StringUpdatePacket
        [TestMethod]
        public void Serialize_StringUpdatePacket()
        {
            RegisterZeroFormatter();

            StringUpdatePacket packet = StringUpdatePacket.Create(0, 1, "hi");
            byte[] data = null;
            StringUpdatePacket.Serialize(packet, out data);

            StringUpdatePacket outPacket = StringUpdatePacket.Deserialize(data);

            Assert.IsTrue(packet.networkID == outPacket.networkID);
            Assert.IsTrue(packet.stringID == outPacket.stringID);
            Assert.IsTrue(packet.value == outPacket.value);
        }

        [TestMethod]
        public void Serialize_StringUpdatePacket_Encrypted()
        {
            RegisterZeroFormatter();
            THEncryption encryption = new THEncryption();

            StringUpdatePacket packet = StringUpdatePacket.Create(0, 1, "hi");
            byte[] data = null;
            StringUpdatePacket.Serialize(packet, out data);

            data = encryption.Encrypt(data);

            data = encryption.Decrypt(data);

            StringUpdatePacket outPacket = StringUpdatePacket.Deserialize(data);

            Assert.IsTrue(packet.networkID == outPacket.networkID);
            Assert.IsTrue(packet.stringID == outPacket.stringID);
            Assert.IsTrue(packet.value == outPacket.value);
        }

        [TestMethod]
        public void Serialize_StringUpdatePacket_Array()
        {
            RegisterZeroFormatter();

            var originArr = new StringUpdatePacket[ARRAY_TEST_COUNT];
            var testArr = new StringUpdatePacket[ARRAY_TEST_COUNT];

            Random r = new Random(DateTime.Now.Millisecond);

            for(int i = 0; i < ARRAY_TEST_COUNT; i++)
            {
                originArr[i] = StringUpdatePacket.Create((uint)r.Next(0, Int32.MaxValue), (byte)r.Next(0,255), DateTime.Now.Millisecond.ToString());
            }

            byte[] data = null;
            for (int i = 0; i < ARRAY_TEST_COUNT; i++)
            {
                data = null;
                StringUpdatePacket.Serialize(originArr[i], out data);
                testArr[i] = StringUpdatePacket.Deserialize(data);
            }

            for (int i = 0; i < ARRAY_TEST_COUNT; i++)
            {
                Assert.IsTrue(originArr[i].networkID == testArr[i].networkID);
                Assert.IsTrue(originArr[i].stringID == testArr[i].stringID);
                Assert.IsTrue(originArr[i].value == testArr[i].value);
            }
        }
        #endregion

        #region RPCPacket
        [TestMethod]
        public void Serialize_RPCPacket()
        {
            RegisterZeroFormatter();

            RPCPacket packet = RPCPacket.Create(0, 1, null);
            RPCPacket.Serialize(packet, out byte[] data);

            RPCPacket outPacket = RPCPacket.Deserialize(data);

            Assert.IsTrue(packet.networkID == outPacket.networkID);
            Assert.IsTrue(packet.RPC_ID == outPacket.RPC_ID);
            Assert.IsTrue(packet.data == outPacket.data);
        }

        [TestMethod]
        public void Serialize_RPCPacket_Encrypted()
        {
            RegisterZeroFormatter();
            THEncryption encryption = new THEncryption();

            RPCPacket packet = RPCPacket.Create(0, 1, null);
            RPCPacket.Serialize(packet, out byte[] data);

            data = encryption.Encrypt(data);

            data = encryption.Decrypt(data);

            RPCPacket outPacket = RPCPacket.Deserialize(data);

            Assert.IsTrue(packet.networkID == outPacket.networkID);
            Assert.IsTrue(packet.RPC_ID == outPacket.RPC_ID);
            Assert.IsTrue(packet.data == outPacket.data);
        }

        [TestMethod]
        public void Serialize_RPCPacket_Array()
        {
            RegisterZeroFormatter();

            var originArr = new RPCPacket[ARRAY_TEST_COUNT];
            var testArr = new RPCPacket[ARRAY_TEST_COUNT];

            Random r = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < ARRAY_TEST_COUNT; i++)
            {
                originArr[i] = RPCPacket.Create((uint)r.Next(0, Int32.MaxValue), (byte)r.Next(0, 255), new byte[] { (byte)r.Next(0, 255), (byte)r.Next(0, 255), (byte)r.Next(0, 255) });
            }

            byte[] data = null;
            for (int i = 0; i < ARRAY_TEST_COUNT; i++)
            {
                data = null;
                RPCPacket.Serialize(originArr[i], out data);
                testArr[i] = RPCPacket.Deserialize(data);
            }

            for (int i = 0; i < ARRAY_TEST_COUNT; i++)
            {
                Assert.IsTrue(originArr[i].networkID == testArr[i].networkID);
                Assert.IsTrue(originArr[i].RPC_ID == testArr[i].RPC_ID);
                Assert.IsTrue(originArr[i].data[0] == testArr[i].data[0]);
                Assert.IsTrue(originArr[i].data[1] == testArr[i].data[1]);
                Assert.IsTrue(originArr[i].data[2] == testArr[i].data[2]);
            }
        }
        #endregion

        #region LoginPacket
        [TestMethod]
        public void Serialize_LoginPacket()
        {
            RegisterZeroFormatter();

            Random r = new Random();

            LoginPacket packet = LoginPacket.Create(0, "User_"+r.Next(0,int.MaxValue), "Pass_" + r.Next(0, int.MaxValue));
            LoginPacket.Serialize(packet, out byte[] data);

            LoginPacket outPacket = LoginPacket.Deserialize(data);

            Assert.IsTrue(packet.networkID == outPacket.networkID);
            Assert.IsTrue(packet.emailHash.Equals(outPacket.emailHash));
            Assert.IsTrue(packet.passwordHash.Equals(outPacket.passwordHash));
        }

        [TestMethod]
        public void Serialize_LoginPacket_Encrypted()
        {
            RegisterZeroFormatter();

            THEncryption encryption = new THEncryption();

            Random r = new Random();

            LoginPacket packet = LoginPacket.Create(0, "User_" + r.Next(0, int.MaxValue), "Pass_" + r.Next(0, int.MaxValue));
            LoginPacket.Serialize(packet, out byte[] data);

            data = encryption.Encrypt(data);

            data = encryption.Decrypt(data);

            LoginPacket outPacket = LoginPacket.Deserialize(data);

            Assert.IsTrue(packet.networkID == outPacket.networkID);
            Assert.IsTrue(packet.emailHash.Equals(outPacket.emailHash));
            Assert.IsTrue(packet.passwordHash.Equals(outPacket.passwordHash));
        }

        [TestMethod]
        public void Serialize_LoginPacket_Array()
        {
            RegisterZeroFormatter();

            var originArr = new LoginPacket[ARRAY_TEST_COUNT];
            var testArr = new LoginPacket[ARRAY_TEST_COUNT];

            Random r = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < ARRAY_TEST_COUNT; i++)
            {
                originArr[i] = LoginPacket.Create(0, "User_" + r.Next(0, int.MaxValue), "Pass_" + r.Next(0, int.MaxValue));
            }

            byte[] data = null;
            for (int i = 0; i < ARRAY_TEST_COUNT; i++)
            {
                data = null;
                LoginPacket.Serialize(originArr[i], out data);
                testArr[i] = LoginPacket.Deserialize(data);
            }

            for (int i = 0; i < ARRAY_TEST_COUNT; i++)
            {
                Assert.IsTrue(originArr[i].networkID == testArr[i].networkID);
                Assert.IsTrue(originArr[i].emailHash.Equals(testArr[i].emailHash));
                Assert.IsTrue(originArr[i].passwordHash.Equals(testArr[i].passwordHash));
            }
        }
        #endregion
    }
}
