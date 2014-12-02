using System;
using System.Collections.Generic;
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
        }
    }
}
