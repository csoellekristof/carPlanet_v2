using CarPlanet.Models;
using CarPlanet.Models.DB;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;

namespace CarPlanet.Controllers {
    public class AutoController : Controller {
        private IRepositoryAuto _rep = new RepositoryAuto();
        public async Task<IActionResult> Autos() {
            try
            {
                await _rep.ConnectAsync();
                return View(await _rep.GetAllAutosAsync());
            }
            catch (DbException)
            {
                return View("_Message", new Message("Datenbankfehler", "Die Benutzer konnten geladen werden", "Versuchen sie es spaeter erneut"));

            }
            finally {
                await _rep.DisconnectAsync();
            }
        }
       
    }
    

}
