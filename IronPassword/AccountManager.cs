using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Security.Cryptography;
using Windows.Storage;
using Windows.Storage.Streams;

namespace IronPassword
{
    class AccountManager
    {
        private String password;
        private IBuffer initVector;

        public List<Account> Accounts {get; private set;}

        public AccountManager(String password)
        {
            this.password = password;
            this.initVector = Crypto.getInitVector();
            Accounts = new List<Account>();
        }

        public static async Task<AccountManager> ReadFromFile(StorageFile accountFile, String password)
        {
            List<Account> accounts = new List<Account>();
            JsonObject fileObject = JsonValue.Parse(await FileIO.ReadTextAsync(accountFile)).GetObject();
            IBuffer initVector = CryptographicBuffer.DecodeFromBase64String(fileObject.GetNamedString("initVector"));
            JsonArray accountsArray = fileObject.GetNamedArray("accounts");
            for (int i = 0; i < accountsArray.Count(); i++)
            {
                IBuffer encryptedAccount = CryptographicBuffer.DecodeFromBase64String(accountsArray.GetStringAt((uint)i));
                IBuffer encodedAccount = Crypto.decrypt(password, initVector, encryptedAccount);
                String serializedAccount = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, encodedAccount);
                Account account = Account.Deserialize(serializedAccount);
                accounts.Add(account);
            }
            AccountManager result = new AccountManager(password);
            result.initVector = initVector;
            result.Accounts = accounts;
            return result;
        }

        public async void WriteToFile(StorageFile accountFile)
        {
            JsonObject fileObject = new JsonObject();
            fileObject.SetNamedValue("initVector", JsonValue.CreateStringValue(CryptographicBuffer.EncodeToBase64String(initVector)));
            JsonArray accountsArray = new JsonArray();
            for (int i = 0; i < Accounts.Count(); i++)
            {
                String serializedAccount = Accounts[i].Serialize();
                IBuffer encodedAccount = CryptographicBuffer.ConvertStringToBinary(serializedAccount, BinaryStringEncoding.Utf8);
                IBuffer encryptedAccount = Crypto.EncryptAsync(password, initVector, encodedAccount);
                String base64Account = CryptographicBuffer.EncodeToBase64String(encryptedAccount);
                accountsArray.Add(JsonValue.CreateStringValue(base64Account));
            }
            fileObject.SetNamedValue("accounts", accountsArray);
            await FileIO.WriteTextAsync(accountFile, fileObject.Stringify());
        }
    }
}
