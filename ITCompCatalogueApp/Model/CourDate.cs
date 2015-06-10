using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ITCompCatalogueApp.Model
{
    public class CourDate
    {
        public int id { get; set; }

        [JsonProperty(PropertyName = "CourID")]
        public int CourID { get; set; }
        [JsonProperty(PropertyName = "DateDebut")]
        public DateTime DateDebut { get; set; }
        [JsonProperty(PropertyName = "DateFin")]
        public DateTime DateFin { get; set; }

      
    }
}
