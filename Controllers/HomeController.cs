using CarPlanet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CarPlanet.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult Privacy() {
            return View();
        }
        public IActionResult Autos() {
            List<Autos> B = AutosListe();
            return View(B);
        }
        public IActionResult Impressum() {
            return View();
        }
        public IActionResult LoginRegister() {
            return View();
        }
        public IActionResult Ersatzteile() {
            List<Ersatzteil> e = TeilListe();
            return View(e);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private List<Autos> AutosListe()
        {
            Autos a1 = new Autos(1, "Mustang", "Ford", new DateTime(1964, 4, 17), 274);
            Autos a2 = new Autos(2, "Skyline GTR", "Nissan", new DateTime(1999, 4, 1), 280);
            Autos a3 = new Autos(3, "P1", "McLaren", new DateTime(2013, 10, 1), 916);

            return new List<Autos>() {
                a1,a2,a3
            };
        }

        private List<Ersatzteil> TeilListe()
        {
            Ersatzteil e1 = new Ersatzteil(1, "Turbocharger",   "ISt ein Kompressor der mithilfe des Abgsstroms mehr Luft treibstoff gemisch in den Motor bringt was in mehr PS resultiert");
            Ersatzteil e2 = new Ersatzteil(2, "Kompressor",  "Ist ein Kompressor der durch den Keilriehmen angetrieben wird und mehr Lut Treibstoffgemisch in den Motor brint wodurch man mehr PS hat");
            Ersatzteil e3 = new Ersatzteil(3, "Federung",  "Sorgt für bessere strassenhaftung und mehr fahrkomvor");

            return new List<Ersatzteil>() {
                e1,e2,e3
            };
        }
    }

}
