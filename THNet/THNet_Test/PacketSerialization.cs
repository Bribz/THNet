using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using THEngine;
using ZeroFormatter;

namespace THNet_Test
{
    [TestClass]
    public class PacketSerialization
    {
        public const int ARRAY_TEST_COUNT = 2000;

        [ClassInitialize]
        public static void AssemblyInit(TestContext context)
        {
            ZeroFormatterInitializer.Register();
        }

        [TestMethod]
        public void Non_Timed()
        {
            Assert.IsTrue(true);
        }

        #region StringUpdatePacket
        [TestMethod]
        public void Serialize_StringUpdatePacket()
        {
            //RegisterZeroFormatter();

            StringUpdatePacket packet = StringUpdatePacket.Create(0, 1, "hi");
            byte[] data = null;
            //StringUpdatePacket.Serialize(packet, out data);
            data = ZeroFormatterSerializer.Serialize<StringUpdatePacket>(packet);

            StringUpdatePacket outPacket = ZeroFormatterSerializer.Deserialize<StringUpdatePacket>(data);//StringUpdatePacket.Deserialize(data);

            Assert.IsTrue(packet.networkID == outPacket.networkID);
            Assert.IsTrue(packet.stringID == outPacket.stringID);
            Assert.IsTrue(packet.value == outPacket.value);
        }

        [TestMethod]
        public void Serialize_StringUpdatePacket_Encrypted()
        {
            //RegisterZeroFormatter();
            THAESEncryption encryption = new THAESEncryption();

            StringUpdatePacket packet = StringUpdatePacket.Create(0, 1, "hi");
            byte[] data = null;
            //StringUpdatePacket.Serialize(packet, out data);
            data = ZeroFormatterSerializer.Serialize<StringUpdatePacket>(packet);

            data = encryption.Encrypt(data);

            data = encryption.Decrypt(data);

            StringUpdatePacket outPacket = ZeroFormatterSerializer.Deserialize<StringUpdatePacket>(data); //StringUpdatePacket.Deserialize(data);

            Assert.IsTrue(packet.networkID == outPacket.networkID);
            Assert.IsTrue(packet.stringID == outPacket.stringID);
            Assert.IsTrue(packet.value == outPacket.value);
        }

        [TestMethod]
        public void Serialize_StringUpdatePacket_Array()
        {
            //RegisterZeroFormatter();

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
                //StringUpdatePacket.Serialize(originArr[i], out data);
                data = ZeroFormatterSerializer.Serialize<StringUpdatePacket>(originArr[i]);

                testArr[i] = ZeroFormatterSerializer.Deserialize<StringUpdatePacket>(data); //StringUpdatePacket.Deserialize(data);
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
            //RegisterZeroFormatter();
            byte[] data;
            RPCPacket packet = RPCPacket.Create(0, 1, null);
            //RPCPacket.Serialize(packet, out byte[] data);
            data = ZeroFormatterSerializer.Serialize<RPCPacket>(packet);

            RPCPacket outPacket = ZeroFormatterSerializer.Deserialize<RPCPacket>(data); //RPCPacket.Deserialize(data);

            Assert.IsTrue(packet.networkID == outPacket.networkID);
            Assert.IsTrue(packet.RPC_ID == outPacket.RPC_ID);
            Assert.IsTrue(packet.data == outPacket.data);
        }

        [TestMethod]
        public void Serialize_RPCPacket_Encrypted()
        {
            //RegisterZeroFormatter();
            THAESEncryption encryption = new THAESEncryption();

            byte[] data;
            RPCPacket packet = RPCPacket.Create(0, 1, null);
            //RPCPacket.Serialize(packet, out byte[] data);
            data = ZeroFormatterSerializer.Serialize<RPCPacket>(packet);

            data = encryption.Encrypt(data);

            data = encryption.Decrypt(data);

            RPCPacket outPacket = ZeroFormatterSerializer.Deserialize<RPCPacket>(data); //RPCPacket.Deserialize(data);

            Assert.IsTrue(packet.networkID == outPacket.networkID);
            Assert.IsTrue(packet.RPC_ID == outPacket.RPC_ID);
            Assert.IsTrue(packet.data == outPacket.data);
        }

        [TestMethod]
        public void Serialize_RPCPacket_Array()
        {
            //RegisterZeroFormatter();

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
                //RPCPacket.Serialize(originArr[i], out data);
                data = ZeroFormatterSerializer.Serialize<RPCPacket>(originArr[i]);
                testArr[i] = ZeroFormatterSerializer.Deserialize<RPCPacket>(data); //RPCPacket.Deserialize(data);
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
            //RegisterZeroFormatter();

            Random r = new Random();

            byte[] data;
            //LoginPacket packet = LoginPacket.Create(0, "User_"+r.Next(0,int.MaxValue), "Pass_" + r.Next(0, int.MaxValue));
            LoginPacket packet = LoginPacket.Create(1, "user", "pass");
            //LoginPacket.Serialize(packet, out byte[] data);
            data = ZeroFormatterSerializer.Serialize<LoginPacket>(packet);

            LoginPacket outPacket = ZeroFormatterSerializer.Deserialize<LoginPacket>(data); //LoginPacket.Deserialize(data);

            Assert.IsTrue(packet.networkID == outPacket.networkID);
            Assert.IsTrue(packet.emailHash.Equals(outPacket.emailHash));
            Assert.IsTrue(packet.passwordHash.Equals(outPacket.passwordHash));
        }

        [TestMethod]
        public void Serialize_LoginPacket_Encrypted()
        {
            //RegisterZeroFormatter();

            THAESEncryption encryption = new THAESEncryption();

            Random r = new Random();

            byte[] data;
            LoginPacket packet = LoginPacket.Create(0, "User_" + r.Next(0, int.MaxValue), "Pass_" + r.Next(0, int.MaxValue));
            //LoginPacket.Serialize(packet, out byte[] data);
            data = ZeroFormatterSerializer.Serialize<LoginPacket>(packet);

            data = encryption.Encrypt(data);

            data = encryption.Decrypt(data);

            LoginPacket outPacket = ZeroFormatterSerializer.Deserialize<LoginPacket>(data); //LoginPacket.Deserialize(data);

            Assert.IsTrue(packet.networkID == outPacket.networkID);
            Assert.IsTrue(packet.emailHash.Equals(outPacket.emailHash));
            Assert.IsTrue(packet.passwordHash.Equals(outPacket.passwordHash));
        }

        [TestMethod]
        public void Serialize_LoginPacket_Array()
        {
            //RegisterZeroFormatter();

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
                //LoginPacket.Serialize(originArr[i], out data);
                data = ZeroFormatterSerializer.Serialize<LoginPacket>(originArr[i]);

                testArr[i] = ZeroFormatterSerializer.Deserialize<LoginPacket>(data);//LoginPacket.Deserialize(data);
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