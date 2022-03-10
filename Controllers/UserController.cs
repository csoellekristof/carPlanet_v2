using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using CarPlanet.Models;
using FirstWebApp.Models.DB;
using Microsoft.AspNetCore.Mvc;
namespace CarPlanet.Controllers
{
    public class UserController : Controller
    {
        private IRepositoryUsers _rep = new RepositoryUsersDB();
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpGet]
        public IActionResult Register() {
            return View();
        }
        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User userDataFromForm)
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
                    _rep.Connect();
                    if (_rep.Insert(userDataFromForm))
                    {
                        return View("Message", new Message("Registrierung", "Ihre Daten wurden erfolgreich abgespeichert"));

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
                    _rep.Disconnect();
                }

                
            }

            //Eingabedaten in  einer DB-Tabelle abspeichern
            //Falls etwas falsch eingegeben wurde wird das Formular nocheinmal angezeigt
            return  View(userDataFromForm);
        }
        [HttpPost]
        public IActionResult Login(User userDataFromForm) {

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
                    _rep.Connect();
                    if (_rep.Login(userDataFromForm.Email, userDataFromForm.Passwort))
                    {
                        return View("Message", new Message("Login", "Sie sind jetzt angemeldet"));

                    }
                    else
                    {
                        return View("Message", new Message("Login", "IHr Passwort oder username ist Falsch!"));

                    }
                }
                catch (DbException)
                {
                    return View("Message", new Message("Login", "Datenbankfehler!", "Bitte versuchen sie es später erneut!"));

                }
                finally
                {
                    _rep.Disconnect();
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
