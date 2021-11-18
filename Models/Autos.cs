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
        public string Autoname { get; set; }

        public string Brand { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int PS { get; set; }

        public Autos(int AutoId, string Autoname, string Brand, DateTime ReleaseDate, int PS)
        {
            this.AutoId = AutoId;
            this.Autoname = Autoname;
            this.Brand = Brand;
            this.ReleaseDate = ReleaseDate;
            this.PS = PS;
        }


    }
}
