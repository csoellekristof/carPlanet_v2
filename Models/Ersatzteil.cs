using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPlanet.Models
{
    public class Ersatzteil
    {
        private int ersatzId;

        public int ErsatzId
        {
            get { return this.ersatzId; }
            set
            {

                if (value >= 0)
                {
                    this.ersatzId = value;
                }
            }
        }
        public string Teilname { get; set; }

        

        public string Nutzen { get; set; }

        public Ersatzteil(int ErsatzId, string Teilname,  string Nutzen)
        {
            this.ErsatzId = ErsatzId;
            this.Teilname = Teilname;
            this.Nutzen = Nutzen;
        }




    }
}
