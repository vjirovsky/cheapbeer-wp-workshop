using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using Windows.UI.Xaml.Shapes;
using CheapBeer.Common;

namespace CheapBeer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        private readonly NavigationHelper navigationHelper;

        public MapPage()
        {
            this.InitializeComponent();

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
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
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
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>.
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            MyMapControl.MapServiceToken = App.MapServiceToken;

            MyMapControl.Loaded += MyMapControlOnLoaded;

        }

        private void CreatePubPushPin(string PubId, string pubName, double latitude, double longitude)
        {
            StackPanel panel = new StackPanel();
            panel.Name = PubId;
            TextBlock pubNameTextBlock = new TextBlock();

            pubNameTextBlock.Foreground = new SolidColorBrush(Colors.Black);
            pubNameTextBlock.FontWeight = FontWeights.Bold;
            pubNameTextBlock.FontSize = 19;
            pubNameTextBlock.Text = pubName;

            Rectangle markerRectangle = new Rectangle();
            markerRectangle.Fill = new SolidColorBrush(Colors.Black);
            markerRectangle.Height = 20;
            markerRectangle.Height = 5;

            // Creating a Polygon Marker  
            Polygon polygon = new Polygon();
            polygon.Points.Add(new Point(0, 0));
            polygon.Points.Add(new Point(0, 50));
            polygon.Points.Add(new Point(25, 0));
            polygon.Fill = new SolidColorBrush(Colors.DarkOrange);


            panel.Children.Add(pubNameTextBlock);
            panel.Children.Add(markerRectangle);
            panel.Children.Add(polygon);

            panel.Tapped += PushPinTapped;

            MyMapControl.Children.Add(panel);
            // Setting up Pushpin location  
            var location = new Geopoint(new BasicGeoposition()
            {
                Latitude = latitude,
                Longitude = longitude
            });
            MapControl.SetLocation(panel, location);
            // Position of the Pushpin  
            MapControl.SetNormalizedAnchorPoint(panel, new Point(0.0, 1.0));  

        }

        private async void PushPinTapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            MessageDialog dialog;
            var panel = sender as StackPanel;
            if (panel == null)
            {

                dialog = new MessageDialog("Neznámá restaurace!");
                await dialog.ShowAsync();

                return;
            }

            var pubId = int.Parse(panel.Name);

            dialog = new MessageDialog("Pub ID = " + pubId);
            await dialog.ShowAsync();
        }

        private void MyMapControlOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {

            if (App.Geoposition != null)
            {
                MyMapControl.Center = App.Geoposition.Coordinate.Point;
                MyMapControl.ZoomLevel = 15;

                var mapIcon = new MapIcon();
                mapIcon.NormalizedAnchorPoint = new Point(0.25, 0.9);
                mapIcon.Location = App.Geoposition.Coordinate.Point;
                mapIcon.Title = "Stojíte zde";

                MyMapControl.MapElements.Add(mapIcon);
            }


            MyMapControl.MapElements.Clear();
            foreach (var pub in App.AllPubsList)
            {
                throw new NotImplementedException();
            }


        }
    }
}
