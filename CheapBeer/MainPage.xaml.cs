using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641
using CheapBeer.Common;
using CheapBeer.Model;
using CheapBeer.ViewModel;

namespace CheapBeer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private readonly NavigationHelper navigationHelper;
        public MainPage()
        {
            this.InitializeComponent();


            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }
        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion




// Work from here down


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
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            #region Geolokace
            var geolocator = new Geolocator();

            bool haveGeoFailed = false;

            try
            {
                App.Geoposition = await geolocator.GetGeopositionAsync();
            }
            catch (Exception ex)
            {
                haveGeoFailed = true;
            }

            if (haveGeoFailed == true)
            {
                var dialog = new MessageDialog("Geolokace selhala!");
                await dialog.ShowAsync();
            }
            else if (App.Geoposition != null)
            {
                BasicGeoposition queryHint = new BasicGeoposition();
                // DALLAS
                queryHint.Latitude = App.Geoposition.Coordinate.Latitude;
                queryHint.Longitude = App.Geoposition.Coordinate.Longitude;

                Geopoint hintPoint = new Geopoint(queryHint);
                
                MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(hintPoint);

                if (result.Status == MapLocationFinderStatus.Success)
                {
                    if (result.Locations != null && result.Locations.Count > 0)
                    {
                        var viewmodel = this.DataContext as MainViewModel;
                        viewmodel.CityName = result.Locations[0].Address.Town;
                    }
                }
            }
            #endregion

            await refreshFromAzureTask();


        }

        private double convertLatitudeAndLongitudeFromStringToDouble(string azureFormat)
        {
            return double.Parse(azureFormat, CultureInfo.InvariantCulture);
        }

        private async Task refreshFromAzureTask()
        {
            StatusBarProgressIndicator progressbar = StatusBar.GetForCurrentView().ProgressIndicator;
            progressbar.Text = "Načítám data z cloudu";
            await progressbar.ShowAsync();

            bool haveAzureFailed = false;
            var viewmodel = (this.DataContext as MainViewModel);
            try
            {
                var pubsAzure =
                    await App.cheapbeerClient.GetTable<AzurePubModel>().OrderBy(u => u.cheapest_beer_price).ToEnumerableAsync();



                var newPubsToDisplay = new Dictionary<string, PubModel>();
                App.AllPubsList.Clear();
                foreach (var pub in pubsAzure)
                {
                    var newPub = new PubModel()
                    {
                        Id = pub.id,
                        Name = pub.name,
                        CheapestBeerName = pub.cheapest_beer_name,
                        CheapestBeerPrice = int.Parse(pub.cheapest_beer_price),
                        Latitude = convertLatitudeAndLongitudeFromStringToDouble(pub.latitude),
                        Longitude = convertLatitudeAndLongitudeFromStringToDouble(pub.longitude)
                    };

                    newPubsToDisplay.Add(pub.id, newPub);
                    App.AllPubsList.Add(newPub);

                }
                viewmodel.PubsToDisplay = newPubsToDisplay;

            }
            catch (Exception ee)
            {
                haveAzureFailed = true;

            }

            if (haveAzureFailed == true)
            {
                var dialog = new MessageDialog("Stahování dat z Azure selhalo!");
                await dialog.ShowAsync();
            }

            await progressbar.HideAsync();
        }

        private async void Appbar_Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Appbar_Map_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Appbar_About_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }


        private async void PubsListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
           var pubId = ((PubModel)e.ClickedItem).Id;

           throw new NotImplementedException();
        }




    }
}
