﻿using IronPassword.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace IronPassword
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class ViewAccountsPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        //private AccountManager manager;

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public ViewAccountsPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            //AccountManager manager = e.NavigationParameter as AccountManager;
            //if (manager == null)
            //{
            //    if (e.PageState == null)
            //    {
            //        this.Frame.GoBack();
            //    }
            //}
            //else
            //{
            //    this.manager = manager;
            //    this.itemGridView.ItemsSource = manager.Accounts;
            //}

            if(AccountManager.Accounts.Count == 0)
            {
                JsonArray jsonArray = AccountManager.safe.json.GetObject().GetNamedArray("accounts");

                for (uint i = 0; i < jsonArray.Count; i++)
                {
                    JsonObject jsonObject = jsonArray.GetObjectAt(i);

                    Account account = new Account();
                    account.ID = (int)jsonObject.GetNamedNumber("id");
                    account.AccountName = jsonObject.GetNamedString("name");
                    account.Username = jsonObject.GetNamedString("username");
                    account.Password = jsonObject.GetNamedString("password");

                    string datetime = jsonObject.GetNamedString("datetime");
                    account.CreationTime = Convert.ToDateTime(datetime);

                    AccountManager.Accounts.Add(account);
                }
            }
            
            this.itemGridView.ItemsSource = AccountManager.Accounts;
            this.itemGridView.InvalidateArrange();
        }

        #region NavigationHelper registration

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void addAccountAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddAccountPage));
        }

        private void aboutAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutPage));
        }

        private void itemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var account = e.ClickedItem as Account;

            this.Frame.Navigate(typeof(ViewSingleAccountPage), account);
        }
    }
}
