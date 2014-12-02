using System;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace IronPassword
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {

        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AccountManager accounts = new AccountManager(passwordBox.Password);
            this.Frame.Navigate(typeof(ViewAccountsPage), accounts);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            StorageFile result = null;
            try
            {
                result = await ApplicationData.Current.RoamingFolder.GetFileAsync(PasswordSafe.PasswordFile);
            }
            catch (FileNotFoundException) {}

            if (result == null)
            {
                this.Frame.Navigate(typeof(CreateMasterPasswordPage));
            }
        }
    }
}
