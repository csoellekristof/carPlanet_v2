using CarPlanet.Models;
using CarPlanet.Models.DB;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;
using Microsoft.AspNetCore.Http;
using System.Dynamic;

namespace CarPlanet.Controllers {
    public class AutoController : Controller {
        private IRepositoryAuto _rep = new RepositoryAuto();
        public async Task<IActionResult> Autos() {
            try
            {
                await _rep.ConnectAsync();
                ViewBag.isAdmin = HttpContext.Session.GetInt32("IsAdmin");
                return View(await _rep.GetAllAutosAsync());
            }
            catch (DbException)
            {
                return View("Message", new Message("Datenbankfehler", "Die Autos konnten nicht geladen werden", "Versuchen sie es spaeter erneut"));

            }
            finally {
                await _rep.DisconnectAsync();
            }
        }

        public IActionResult InsertPartial() {
            return View();
        }

        public async Task<IActionResult> Delete(int id) {
            try
            {
                await _rep.ConnectAsync();
                await _rep.DeleteAsync(id);
                return RedirectToAction("Autos");
            }
            catch (DbException e) {
                return View("Message", new Message("Datenbankfehler", e.StackTrace));
            }
            finally
            {
                await _rep.DisconnectAsync();
            }

        }

        public async Task<IActionResult> Insert(Autos autoFromForm) {

            try
            {
                await _rep.ConnectAsync();
                await _rep.InsertAsync(autoFromForm);
                return RedirectToAction("Autos");
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
