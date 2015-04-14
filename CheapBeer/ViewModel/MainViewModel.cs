using System.Collections.Generic;
using CheapBeer.Model;
using GalaSoft.MvvmLight;

namespace CheapBeer.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        /// <summary>
        /// The <see cref="CityName" /> property's name.
        /// </summary>
        public const string CityNamePropertyName = "CityName";

        private string _cityName = "";

        /// <summary>
        /// Sets and gets the CityName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string CityName
        {
            get
            {
                return _cityName;
            }
            set
            {
                Set(CityNamePropertyName, ref _cityName, value);
            }
        }

        /// <summary>
        /// The <see cref="PubsToDisplay" /> property's name.
        /// </summary>
        public const string PubsToDisplayPropertyName = "PubsToDisplay";

        private Dictionary<string, PubModel> _pubsToDisplayList = new Dictionary<string, PubModel>();

        /// <summary>
        /// Sets and gets the PubsToDisplay property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Dictionary<string, PubModel> PubsToDisplay
        {
            get
            {
                return _pubsToDisplayList;
            }
            set
            {
                Set(PubsToDisplayPropertyName, ref _pubsToDisplayList, value);
            }
        }


        public bool AddPubToDisplay(PubModel pubItem)
        {
            PubsToDisplay.Add(pubItem.Id, pubItem);
            RaisePropertyChanged(PubsToDisplayPropertyName);
            return true;
        }


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in designer
                CityName = "Prague";

                for (int i = 0; i < 20; i++)
                {
                    PubsToDisplay.Add(i.ToString(),new PubModel()
                    {
                        Id = i.ToString(),
                        Name = "Moe's Tavern",
                        CheapestBeerName = "Duff Cheap",
                        CheapestBeerPrice = 29
                    });
                }
            }
        }
    }
}