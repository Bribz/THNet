using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace THEngine
{
    public class THAESEncryption
    {
        private static byte[] key = { 21, 208, 230, 153, 123, 84, 122, 20, 209, 105, 62, 195, 39, 72, 126, 12, 91, 81, 76, 245, 249, 225, 54, 63, 92, 171, 238, 130, 156, 75, 68, 21 };

        // a hardcoded IV should not be used for production AES-CBC code
        // IVs should be unpredictable per ciphertext
        private static byte[] vector = { 19, 183, 244, 84, 145, 140, 145, 47, 10, 163, 111, 31, 24, 199, 208, 219 };

        private ICryptoTransform encryptor, decryptor;
        private UTF8Encoding encoder;

        public THAESEncryption()
        {
            RijndaelManaged rm = new RijndaelManaged();
            encryptor = rm.CreateEncryptor(key, vector);
            decryptor = rm.CreateDecryptor(key, vector);
            encoder = new UTF8Encoding();
        }

        public string Encrypt(string unencrypted)
        {
        return Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
        }

        public string Decrypt(string encrypted)
        {
        return encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
        }

        public byte[] Encrypt(byte[] buffer)
        {
        return Transform(buffer, encryptor);
        }

        public byte[] Decrypt(byte[] buffer)
        {
        return Transform(buffer, decryptor);
        }

        protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
        MemoryStream stream = new MemoryStream();
        using (CryptoStream cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
        {
            cs.Write(buffer, 0, buffer.Length);
        }
        return stream.ToArray();
        }
    }

    public class THHash
    {
        public enum HashType
        {
            SHA1,
            SHA256,
            SHA384,
            SHA512,
            MD5
        }

        public static byte[] ComputeHash(string data, HashType algorithm = HashType.SHA256)
        {
            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(data);
            
            // Because we support multiple hashing algorithms, we must define
            // hash object as a common (abstract) base class. We will specify the
            // actual hashing algorithm class later during object creation.
            HashAlgorithm hash;
            
            // Initialize appropriate hashing algorithm class.
            switch (algorithm)
            {
                case HashType.SHA1:
                    hash = new SHA1Managed();
                    break;

                case HashType.SHA256:
                    hash = new SHA256Managed();
                    break;

                case HashType.SHA384:
                    hash = new SHA384Managed();
                    break;

                case HashType.SHA512:
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);

            return hashBytes;
        }

        public static byte[] ComputeHash(byte[] data, HashType algorithm = HashType.SHA256)
        {

            // Because we support multiple hashing algorithms, we must define
            // hash object as a common (abstract) base class. We will specify the
            // actual hashing algorithm class later during object creation.
            HashAlgorithm hash;

            // Initialize appropriate hashing algorithm class.
            switch (algorithm)
            {
                case HashType.SHA1:
                    hash = new SHA1Managed();
                    break;

                case HashType.SHA256:
                    hash = new SHA256Managed();
                    break;

                case HashType.SHA384:
                    hash = new SHA384Managed();
                    break;

                case HashType.SHA512:
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(data);

            return hashBytes;
        }
    }
}
