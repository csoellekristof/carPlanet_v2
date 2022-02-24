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
        public IActionResult Register() {
            return View();
        }
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
                return RedirectToAction("LoginRegister");
            }

            //Eingaben des Benutzers Überprüfen - Validierung

            ValidateRegistrationData(userDataFromForm);
            //fals das Formular richtig ausgefüllt wurde
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            //Eingabedaten in  einer DB-Tabelle abspeichern
            //Falls etwas falsch eingegeben wurde wird das Formular nocheinmal angezeigt
            return View(userDataFromForm);
        }

        public IActionResult Login(User userDataFromForm) {

            //Parameter Überprüfen
            if (userDataFromForm == null)
            {
                //weiteleitung an eine Methode (Action) in selben Controller
                return RedirectToAction("LoginRegister");
            }
            ValidateLoginData(userDataFromForm);
            //fals das Formular richtig ausgefüllt wurde
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");   
            }

            //Eingabedaten in  einer DB-Tabelle abspeichern
            //Falls etwas falsch eingegeben wurde wird das Formular nocheinmal angezeigt
            return View(userDataFromForm);


        }

        private void ValidateRegistrationData(User u)
        {
            //Parameter Überprüfen
            if (u == null)
            {
                return;
            }
            //Username
            try
            {
                if (!u.Email.Contains("@"))
                {
                    ModelState.AddModelError("EMail", "Die EMail sollte in dem EMail-Format (bsp.: maxmustermann@abc.com)");
                }
            }
            catch (NullReferenceException e) {
                ModelState.AddModelError("EMail", "Feld muss ausgefüllt werden!");
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
                ModelState.AddModelError("Passwort", "Das Passwort muss Grosbuchstaben und Kleinbuchstaben enthalten");

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

        private void ValidateLoginData( User u) {

           
        }
    }
}
