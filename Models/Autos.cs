using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CarPlanet.Models
{
    public class Autos
    {
        private int autoId;

        public int AutoId
        {
            get { return this.autoId; }
            set
            {

                if (value >= 0)
                {
                    this.autoId = value;
                }
            }
        }
        public string Name { get; set; }

        public string Beschreibung { get; set; }

        public string Typ { get; set; }

        public string Link { get; set; }


        public Autos(int AutoId, string Name, string Beschreibung, string Typ, string Link)
        {
            this.AutoId = AutoId;
            this.Autoname = Autoname;
            this.Brand = Brand;
            this.ReleaseDate = ReleaseDate;
            this.PS = PS;
        }

        public Autos()
        {
        }
    }
}
