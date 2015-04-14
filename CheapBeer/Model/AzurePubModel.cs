using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace CheapBeer.Model
{

    [DataTable("pubs")]
    public class AzurePubModel
    {
        public string id;
        public string name;
        public string cheapest_beer_name;
        public string cheapest_beer_price;
        public string latitude;
        public string longitude;
        public string address;
        public string open_hours;


    }
}
