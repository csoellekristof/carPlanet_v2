using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarPlanet.Models;
using Microsoft.AspNetCore.Mvc;
namespace CarPlanet.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            
            
            return View();
        }

        [HttpGet]
        public IActionResult LoginRegister()
        {

            return View();
        }

        [HttpPost]
        public IActionResult LoginRegister(User userDaterFromForm)
        {
            //Parameter Überprüfen
            if (userDaterFromForm == null)
            {
                //weiteleitung an eine Methode (Action) in selben Controller
                return RedirectToAction("LoginRegister");
            }

            //Eingaben des Benutzers Überprüfen - Validierung

            ValidateRegistrationData(userDaterFromForm);
            //fals das Formular richtig ausgefüllt wurde
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            //Eingabedaten in  einer DB-Tabelle abspeichern
            //Falls etwas falsch eingegeben wurde wird das Formular nocheinmal angezeigt
            return View(userDaterFromForm);
        }

        private void ValidateRegistrationData(User u)
        {
            //Parameter Überprüfen
            if (u == null)
            {
                return;
            }
            //Username

            if (!u.Email.Contains("@")|| u.Email == null)
            {
                ModelState.AddModelError("EMail", "Die EMail sollte in dem EMail-Format (bsp.: maxmustermann@abc.com)");
            }
            //Passwort
            Boolean Kleinbuchstabe = false;
            Boolean Großbuchstabe = false;
            if (u.Passwort == null || (u.Passwort.Length < 8))
            {
                ModelState.AddModelError("Passwort", "Das Passwort muss mindestens 8 zeichen lang sein");

            }
            else { 

            string password = u.Passwort;
            Großbuchstabe = !password.ToLower().Equals(password);
            Kleinbuchstabe = !password.ToUpper().Equals(password);
            }
            if(Kleinbuchstabe == false || Großbuchstabe == false)
            {
                ModelState.AddModelError("Passwort", "Das Passwort muss Grosbuchsdtaben und Kleinbuchstaben enthalten");

            }
            //Plus mindestens ein Grosbuchstabe einem Grosbuchstaben einer Zahl + sonderzeichen
            //Email

            //Geburtsdatum
            if (u.Birthdate >= DateTime.Now)
            {
                ModelState.AddModelError("Birthdate", "Das Geburtsdatum muss mindestens 4 zeichen lang sein");


            }

            //Gender
        }
    }
}
