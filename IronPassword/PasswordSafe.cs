using System;
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
        }

        private async void getStorageFile()
        {
            file = await ApplicationData.Current.RoamingFolder.GetFileAsync(PasswordFile);
            string text = await FileIO.ReadTextAsync(file);

            json = JsonValue.Parse(text);
            Password = json.GetObject().GetNamedString("master");
        }

        public async static void initializePasswordFile(string password)
        {
            StorageFile passwordFile = await ApplicationData.Current.RoamingFolder.CreateFileAsync(PasswordFile);

            JsonObject json = new JsonObject();
            json["master"] = JsonValue.CreateStringValue(password);
            json["accounts"] = new JsonArray();

            string jsonText = json.Stringify();

            try
            {
                if(passwordFile != null)
                    await FileIO.WriteTextAsync(passwordFile, jsonText);
            }
            catch (FileNotFoundException) { }
        }

        public int getNextID()
        {
            JsonArray jsonArray = json.GetObject().GetNamedArray("accounts");
            uint size = (uint)jsonArray.Count;

            int nextID;

            if(size == 0)
            {
                nextID = 1;
            }
            else
            {
                uint last = size - 1;
                JsonObject jsonObject = jsonArray.GetObjectAt(last);
                nextID = (int)jsonObject.GetNamedNumber("id") + 1;
            }

            return nextID;
        }

        public void AddAccount(Account account)
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject["id"] = JsonValue.CreateNumberValue(account.ID);
            jsonObject["name"] = JsonValue.CreateStringValue(account.AccountName);
            jsonObject["username"] = JsonValue.CreateStringValue(account.Username);
            jsonObject["password"] = JsonValue.CreateStringValue(account.Password);

            string datetime = Convert.ToString(account.CreationTime);
            jsonObject["datetime"] = JsonValue.CreateStringValue(datetime);

            JsonArray jsonArray = json.GetObject().GetNamedArray("accounts");
            jsonArray.Add(jsonObject);

            WriteToFile();
        }

        public void DeleteAccount(Account account)
        {
            JsonArray jsonArray = json.GetObject().GetNamedArray("accounts");

            bool accountDeleted = false;

            for (uint i = 0; i < jsonArray.Count; i++)
            {
                JsonObject jsonObject = jsonArray.GetObjectAt(i);

                if (!accountDeleted)
                {
                    if ((int)jsonObject.GetNamedNumber("id") == account.ID)
                    {
                        jsonArray.RemoveAt((int)i);
                        accountDeleted = true;
                    }
                }
                else
                {
                    jsonObject["id"] = JsonValue.CreateNumberValue(jsonObject.GetNamedNumber("id") - 1);
                    jsonArray.RemoveAt((int)i);
                    jsonArray.Insert((int)i, jsonObject);
                }
            }

            WriteToFile();
        }

        public void EditAccount(Account account)
        {
            JsonArray jsonArray = json.GetObject().GetNamedArray("accounts");

            for(uint i = 0; i < jsonArray.Count; i++)
            {
                JsonObject oldJsonObject = jsonArray.GetObjectAt(i);

                if((int)oldJsonObject.GetNamedNumber("id") == account.ID)
                {
                    jsonArray.RemoveAt((int)i);

                    JsonObject jsonObject = new JsonObject();
                    jsonObject["id"] = JsonValue.CreateNumberValue(account.ID);
                    jsonObject["name"] = JsonValue.CreateStringValue(account.AccountName);
                    jsonObject["username"] = JsonValue.CreateStringValue(account.Username);
                    jsonObject["password"] = JsonValue.CreateStringValue(account.Password);

                    string datetime = Convert.ToString(account.CreationTime);
                    jsonObject["datetime"] = JsonValue.CreateStringValue(datetime);

                    jsonArray.Insert((int)i, jsonObject);
                }
            }
        }

        public async void WriteToFile()
        {
            string text = json.Stringify();

            try
            {
                if (file != null)
                    await FileIO.WriteTextAsync(file, text);
            }
            catch (FileNotFoundException) { }
        }
    }
}
