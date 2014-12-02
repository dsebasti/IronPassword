using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace IronPassword
{
    class Crypto
    {
        private static IBuffer salt = CryptographicBuffer.ConvertStringToBinary(")GS78g-897g3SP-98whiof", BinaryStringEncoding.Utf8);
        private static uint iterations = 50000;
        private static uint keyLength = 64;

        private static SymmetricKeyAlgorithmProvider keyProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
        private static KeyDerivationParameters keyParams = KeyDerivationParameters.BuildForPbkdf2(salt, iterations);

        public static IBuffer getInitVector()
        {
            return CryptographicBuffer.GenerateRandom(keyProvider.BlockLength);
        }

        public static IBuffer encrypt(String password, IBuffer initVector, IBuffer plaintext)
        {
            CryptographicKey key = buildKey(password);
            return CryptographicEngine.Encrypt(key, plaintext, initVector);
        }

        public static IBuffer decrypt(String password, IBuffer initVector, IBuffer cyphertext)
        {
            CryptographicKey key = buildKey(password);
            return CryptographicEngine.Decrypt(key, cyphertext, initVector);
        }

        private static CryptographicKey buildKey(String password)
        {
            IBuffer secret = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8);
            CryptographicKey initKey = keyProvider.CreateSymmetricKey(secret);
            IBuffer keyMaterial = CryptographicEngine.DeriveKeyMaterial(
                 initKey,
                 keyParams,
                 keyLength);
            return keyProvider.CreateSymmetricKey(keyMaterial);
        }
    }
}
