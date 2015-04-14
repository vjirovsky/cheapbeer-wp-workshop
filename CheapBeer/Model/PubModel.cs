using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace CheapBeer.Model
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class PubModel : ViewModelBase
    {

        /// <summary>
        /// The <see cref="Id" /> property's name.
        /// </summary>
        public const string IdPropertyName = "Id";

        private string _id = "0";

        /// <summary>
        /// Sets and gets the Id property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                Set(IdPropertyName, ref _id, value);
            }
        }

        /// <summary>
        /// The <see cref="Name" /> property's name.
        /// </summary>
        public const string NamePropertyName = "Name";

        private string _name = "";

        /// <summary>
        /// Sets and gets the Name property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                Set(NamePropertyName, ref _name, value);
            }
        }


        /// <summary>
        /// The <see cref="CheapestBeerName" /> property's name.
        /// </summary>
        public const string CheapestBeerNamePropertyName = "CheapestBeerName";

        private string _cheapestBeerName = "";

        /// <summary>
        /// Sets and gets the CheapestBeerName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string CheapestBeerName
        {
            get
            {
                return _cheapestBeerName;
            }
            set
            {
                Set(CheapestBeerNamePropertyName, ref _cheapestBeerName, value);
            }
        }

        /// <summary>
        /// The <see cref="CheapestBeerPrice" /> property's name.
        /// </summary>
        public const string CheapestBeerPricePropertyName = "CheapestBeerPrice";
        public const string CheapestBeerPriceWithSymbolPropertyName = "CheapestBeerPriceWithSymbol";

        private int _cheapestBeerPrice = 0;

        /// <summary>
        /// Sets and gets the CheapestBeerPrice property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int CheapestBeerPrice
        {
            get
            {
                return _cheapestBeerPrice;
            }
            set
            {
                Set(CheapestBeerPricePropertyName, ref _cheapestBeerPrice, value);
                RaisePropertyChanged(CheapestBeerPriceWithSymbolPropertyName);
            }
        }

        public string CheapestBeerPriceWithSymbol
        {
            get { return _cheapestBeerPrice + " Kč"; }
        }

        /// <summary>
        /// The <see cref="Latitude" /> property's name.
        /// </summary>
        public const string LatitudePropertyName = "Latitude";

        private double _latitude = 0;

        /// <summary>
        /// Sets and gets the Latitude property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                Set(LatitudePropertyName, ref _latitude, value);
            }
        }

        /// <summary>
        /// The <see cref="Longitude" /> property's name.
        /// </summary>
        public const string LongitudePropertyName = "Longitude";

        private double _longitude = 0;

        /// <summary>
        /// Sets and gets the Longitude property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                Set(LongitudePropertyName, ref _longitude, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the PubModel class.
        /// </summary>
        public PubModel()
        {
        }

    }
}
