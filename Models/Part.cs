using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPlanet.Models {
    public class Part {

        private int partID;

        public int PartID {
            get { return this.partID; }
            set {

                if (value >= 0)
                {
                    this.partID = value;
                }
            }
        }
        public string Name { get; set; }

        public string Description { get; set; }


        public string Link { get; set; }


        public Part(int AutoId, string Name, string Description, string Link) {
            this.PartID = PartID;
            this.Name = Name;
            this.Description = Description;
            this.Link = Link;
        }

        public Part() {
        }
    }


}

