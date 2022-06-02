using CarPlanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPlanet.Models
{
    public class User
    {
        private int userId;

        public int UserID
        {
            get
            {
                return this.userId;
            }
            set
            {
                if (value >= 0)
                {
                    this.userId = value;
                }
            }
        }
        

        public string Passwort { get; set; }

        public string Email { get; set; }

        public DateTime Birthdate { get; set; }

        public Gender Gender { get; set; }

        public bool IsAdmin { get; set; }

        public bool AGB { get; set; }


    }
}
