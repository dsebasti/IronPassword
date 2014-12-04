using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;

namespace IronPassword
{
    public static class PasswordGenerator
    {
        // http://stackoverflow.com/questions/14582008/what-cryptographically-secure-options-are-there-for-creating-random-numbers-in-w

        public enum PasswordScore
        {
            Blank = 0,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }

        public static string Generate()
        {
            uint length = 12;

            IBuffer randomBuffer = CryptographicBuffer.GenerateRandom(length);
            string randomString = CryptographicBuffer.EncodeToBase64String(randomBuffer);

            return randomString;
        }

        public static int CheckPasswordStrength(string password)
        {
            int score = 1;

            if (password.Length >= 8)
                score++;
            if (password.Length >= 12)
                score++;
            if (Regex.Match(password, @"[0-9]+(\.[0-9][0-9]?)?", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"^(?=.*[a-z])(?=.*[A-Z]).+$", RegexOptions.ECMAScript).Success &&
              Regex.Match(password, @"/[A-Z]/", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript).Success)
                score++;

            return score;
        }
    }
}
