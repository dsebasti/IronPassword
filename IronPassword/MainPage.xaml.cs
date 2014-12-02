using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace IronPassword
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {


        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ViewAccountsPage), passwordBox.Password);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            StorageFile result = null;
            try
            {
                result = await ApplicationData.Current.RoamingFolder.GetFileAsync("passwords.json");
            }
            catch (FileNotFoundException) { }

            if (result == null)
            {
                this.Frame.Navigate(typeof(CreateMasterPasswordPage));
            }
        }
    }
}
