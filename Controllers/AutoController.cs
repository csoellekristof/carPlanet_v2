using CarPlanet.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPlanet.Controllers {
    public class AutoController : Controller {
        public IActionResult Index() {
            List<Autos> B = AutosListe();
            return View();
        }
        private List<Autos> AutosListe()
        {
            Autos a1 = new Autos(1, "Mustang", "Ford", new DateTime(17, 4, 1964), 274);
            Autos a2 = new Autos(2, "Skyline GTR", "Nissan", new DateTime(1, 4, 1999), 280);
            Autos a3 = new Autos(3, "P1", "McLaren", new DateTime(1, 10, 2013), 916);

            return new List<Autos>() {
                a1,a2,a3
            };
        }
    }
    

}
