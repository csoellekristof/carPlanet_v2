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

        Task ConnectAsync();
        Task DisconnectAsync();

        //CRUD_Operationen . . . Create Read Update Delete

        Task<bool> InsertAsync(User user);
        Task<bool> DeleteAsync(int userId);
        Task<bool> ChangeUserDataAsync(int userId, User newUserData);

        Task<User> GetUserAsync(int userId);

        Task<bool> GetEmailAsync(string email);
        Task<List<User>> GetAllUsersAsync();

        Task<bool> LoginAsync(string email, string password);

        //Weitere Methoden
    }
}
