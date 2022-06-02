using CarPlanet.Models;
using CarPlanet.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace CarPlanet.Controllers {
    public class PartController : Controller {

        IRepositoryParts _rep = new RepositoryPartsDB();
        public IActionResult Index() {
            return View();
        }

        public async Task <IActionResult> CompatibleParts(int id) {

            try
            {
                await _rep.ConnectAsync();
                ViewBag.autoID = id;
                ViewBag.isAdmin = HttpContext.Session.GetInt32("IsAdmin");
                return View(await _rep.GetCompatiblePartsAsync(id));
            }
            catch (DbException)
            {
                return View("Message", new Message("Datenbankfehler", "Die Ersatzteile konnten nicht geladen werden", "Versuchen sie es spaeter erneut"));

            }
            finally
            {
                await _rep.DisconnectAsync();
            }


        }

        public async Task<IActionResult> Delete(int id) {
            try
            {
                await _rep.ConnectAsync();
                await _rep.DeleteAsync(id);
                return RedirectToAction("CompatibleParts");
            }
            catch (DbException e)
            {
                return View("Message", new Message("Datenbankfehler", e.StackTrace));
            }
            finally
            {
                await _rep.DisconnectAsync();
            }

        }


    }
}
