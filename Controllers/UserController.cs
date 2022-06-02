using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using CarPlanet.Models;
using FirstWebApp.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace CarPlanet.Controllers
{
    public class UserController : Controller
    {
        private IRepositoryUsers _rep = new RepositoryUsersDB(new HttpContextAccessor());
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpGet]
        public IActionResult Register() {
            if (HttpContext.Session.GetString("Username") != null)
            {
                return View("Logout");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Login() {
            if (HttpContext.Session.GetString("Username") != null) {
                return View("Logout");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CheckEmail(string email)
        {
            

            try
            {
                await _rep.ConnectAsync();
                if (await _rep.GetEmailAsync(email))
                {
                    return new JsonResult(true);

                }
                else
                {
                    return new JsonResult(false);
                }
            }
            catch (DbException)
            {
                return View("Message", new Message("JavaScript", "Datenbankfehler!", "Bitte versuchen sie es später erneut!"));

            }
            finally
            {
                await _rep.DisconnectAsync();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(User userDataFromForm)
        {
            //Parameter Überprüfen
            if (userDataFromForm == null)
            {
                //weiteleitung an eine Methode (Action) in selben Controller
                return RedirectToAction("Register");
            }

            //Eingaben des Benutzers Überprüfen - Validierung

            ValidateRegistrationData(userDataFromForm);
            //fals das Formular richtig ausgefüllt wurde
            if (ModelState.IsValid)
            {
                try
                {
                    await _rep.ConnectAsync();
                    if (await _rep.InsertAsync(userDataFromForm))
                    {
                        return View("Login");

                    }
                    else
                    {
                        return View("Message", new Message("Registrierung", "Ihre Daten wurden nicht erfolgreich abgespeichert", "Bitte versuchen sie es später erneut!"));

                    }
                }
                catch (DbException)
                {
                    return View("Message", new Message("Registrierung", "Datenbankfehler!", "Bitte versuchen sie es später erneut!"));

                }
                finally
                {
                    await _rep.DisconnectAsync();
                }

                
            }
            
            //Eingabedaten in  einer DB-Tabelle abspeichern
            //Falls etwas falsch eingegeben wurde wird das Formular nocheinmal angezeigt
            return  View(userDataFromForm);
        }
        [HttpPost]
        public async Task<IActionResult> Login(User userDataFromForm) {

            //Parameter Überprüfen
            if (userDataFromForm == null)
            {
                //weiteleitung an eine Methode (Action) in selben Controller
                return RedirectToAction("Login");
            }
            ValidateLoginData(userDataFromForm);
            //fals das Formular richtig ausgefüllt wurde
            if (ModelState.IsValid)
            {
                try
                {
                    await _rep.ConnectAsync();
                    if (await _rep.LoginAsync(userDataFromForm.Email, userDataFromForm.Passwort))
                    {
                       
                        return RedirectToAction("Index","Home");

                    }
                    else
                    {
                        return View("Message", new Message("Login", "Ihr Passwort oder username ist Falsch!"));

                    }
                }
                catch (DbException)
                {
                    return View("Message", new Message("Login", "Datenbankfehler!", "Bitte versuchen sie es später erneut!"));

                }
                finally
                {
                    await _rep.DisconnectAsync();
                }
            }

            //Eingabedaten in  einer DB-Tabelle abspeichern
            //Falls etwas falsch eingegeben wurde wird das Formular nocheinmal angezeigt
            return View(userDataFromForm);


        }





        private void ValidateLoginData(User u)
        {
            if (u == null)
            {
                return;
            }
            //Username

            if (u.Email == null || !u.Email.Contains("@"))
            {
                ModelState.AddModelError("EMail", "Die EMail sollte in dem EMail-Format (bsp.: maxmustermann@abc.com)");
            }

            //Passwort
            Boolean Kleinbuchstabe = false;
            Boolean Großbuchstabe = false;

            string password = u.Passwort;
            if (u.Passwort == null || (u.Passwort.Length < 8))
            {
                ModelState.AddModelError("Passwort", "Das Passwort muss mindestens 8 zeichen lang sein");

            }
            else
            {



                Großbuchstabe = !password.ToLower().Equals(password);
                Kleinbuchstabe = !password.ToUpper().Equals(password);
            }
            if (Kleinbuchstabe == false || Großbuchstabe == false)
            {
                ModelState.AddModelError("Passwort", "Das Passwort muss Grosbuchsdtaben und Kleinbuchstaben enthalten");
            }
        }
        private void ValidateRegistrationData(User u)
        {
            //Parameter Überprüfen
            if (u == null)
            {
                return;
            }
            //Username
            
            if (u.Email == null || !u.Email.Contains("@"))
                {
                    ModelState.AddModelError("EMail", "Die EMail sollte in dem EMail-Format (bsp.: maxmustermann@abc.com)");
                }
            
            //Passwort
            Boolean Kleinbuchstabe = false;
            Boolean Großbuchstabe = false;
            
            string password = u.Passwort;
            if (u.Passwort == null || (u.Passwort.Length < 8))
            {
                ModelState.AddModelError("Passwort", "Das Passwort muss mindestens 8 zeichen lang sein");

            }
            else {

                

                Großbuchstabe = !password.ToLower().Equals(password);
            Kleinbuchstabe = !password.ToUpper().Equals(password);
            }
            if(Kleinbuchstabe == false || Großbuchstabe == false  )
            {
                ModelState.AddModelError("Passwort", "Das Passwort muss Grosbuchsdtaben und Kleinbuchstaben enthalten");
            }
           






            //Geburtsdatum
            if (u.Birthdate >= DateTime.Now)
            {
                ModelState.AddModelError("Birthdate", "Das Geburtsdatum muss mindestens 4 zeichen lang sein");


            }

            if(u.AGB == false)
            {
                ModelState.AddModelError("AGB", "Sie müssen die AGBs Akzeptieren");

            }

            //Gender
        }

        
    }
}
