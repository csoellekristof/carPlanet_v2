using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPlanet.Models.DB.sql
{
   interface IRepositoryAuto
    {
        Task ConnectAsync();
        Task DisconnectAsync();

        //CRUD_Operationen . . . Create Read Update Delete

        Task<bool> InsertAsync(Autos auto);
        Task<bool> DeleteAsync(int autoId);
        Task<bool> ChangeAutoDataAsync(int autoId, Autos newAutoData);

        Task<Autos> GetAutoAsync(int autoId);
        Task<List<Autos>> GetAllAutosAsync();
    }
}
