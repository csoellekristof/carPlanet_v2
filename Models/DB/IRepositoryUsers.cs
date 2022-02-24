using CarPlanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebApp.Models.DB
{
    //TODO: asynchrone Programmierung
    //wichtig: es sollte immer gegen eine schnitstelle oder basisklasse Programiert werden
    // daraus folgt programm ist leichter wartbar, änderbar und testbar
    interface IRepositoryUsers
    {

        void Connect();
        void Disconnect();

        //CRUD_Operationen . . . Create Read Update Delete

        bool Insert(User user);
        bool Delete(int userId);
        bool ChangeUserData(int userId, User newUserData);

        User GetUser(int userId);
        List<User> GetAllUsers();

        bool Login(string email, string password);

        //Weitere Methoden
    }
}
