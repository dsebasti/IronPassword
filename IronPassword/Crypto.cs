using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;

namespace IronPassword
{
    public static class Crypto
    {
        //private static IBuffer salt = CryptographicBuffer.ConvertStringToBinary(")GS78g-897g3SP-98whiof", BinaryStringEncoding.Utf8);
        //private static uint iterations = 50000;
        //private static uint keyLength = 64;

        //private static SymmetricKeyAlgorithmProvider keyProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
        //private static KeyDerivationParameters keyParams = KeyDerivationParameters.BuildForPbkdf2(salt, iterations);

        //public static IBuffer getInitVector()
        //{
        //    return CryptographicBuffer.GenerateRandom(keyProvider.BlockLength);
        //}

        //public static IBuffer encrypt(String password, IBuffer initVector, IBuffer plaintext)
        //{
        //    CryptographicKey key = buildKey(password);
        //    return CryptographicEngine.Encrypt(key, plaintext, initVector);
        //}

        //public static IBuffer decrypt(String password, IBuffer initVector, IBuffer cyphertext)
        //{
        //    CryptographicKey key = buildKey(password);
        //    return CryptographicEngine.Decrypt(key, cyphertext, initVector);
        //}

        //private static CryptographicKey buildKey(String password)
        //{
        //    IBuffer secret = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8);
        //    CryptographicKey initKey = keyProvider.CreateSymmetricKey(secret);
        //    IBuffer keyMaterial = CryptographicEngine.DeriveKeyMaterial(
        //         initKey,
        //         keyParams,
        //         keyLength);
        //    return keyProvider.CreateSymmetricKey(keyMaterial);
        //}

        public async static Task<string> EncryptAsync(string text)
        {
            DataProtectionProvider provider = new DataProtectionProvider("LOCAL=user");

            IBuffer message = CryptographicBuffer.ConvertStringToBinary(text, BinaryStringEncoding.Utf16LE);
            IBuffer encrypted = await provider.ProtectAsync(message);

            string result = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf16LE, encrypted);
            return result;
        }

        public async static Task<string> DecryptAsync(string text)
        {
            DataProtectionProvider provider = new DataProtectionProvider();

            IBuffer encrypted = CryptographicBuffer.ConvertStringToBinary(text, BinaryStringEncoding.Utf16LE);
            IBuffer message = await provider.UnprotectAsync(encrypted);

            string result = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf16LE, message);
            return result;
        }
    }
}
