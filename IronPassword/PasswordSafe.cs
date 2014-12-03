﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace IronPassword
{
    public class PasswordSafe
    {
        public static string PasswordFile = "lasHFRfer34tgi3923DFasf";
        public string Password;
        public JsonValue json;

        private StorageFile file;

        public PasswordSafe()
        {
            getStorageFile();
            decryptStorageFile();   
        }

        private async void getStorageFile()
        {
            file = await ApplicationData.Current.RoamingFolder.GetFileAsync(PasswordFile);
        }

        private async void decryptStorageFile()
        {
            string encrypted = await FileIO.ReadTextAsync(file);
            string decrypted = await Crypto.DecryptAsync(encrypted);

            json = JsonValue.Parse(decrypted);
            Password = json.GetObject().GetNamedString("master");
        }

        public async static void initializePasswordFile(string password)
        {
            StorageFile passwordFile = await ApplicationData.Current.RoamingFolder.CreateFileAsync(PasswordFile);

            JsonObject json = new JsonObject();
            json["master"] = JsonValue.CreateStringValue(password);
            json["accounts"] = new JsonArray();

            string jsonText = json.Stringify();
            string encryptedText = await Crypto.EncryptAsync(jsonText);

            try
            {
                if(passwordFile != null)
                    await FileIO.WriteTextAsync(passwordFile, encryptedText, Windows.Storage.Streams.UnicodeEncoding.Utf16LE);
            }
            catch (FileNotFoundException) { }
        }
    }
}
