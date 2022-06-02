using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPlanet.Models.DB {
    interface IRepositoryParts {

        Task ConnectAsync();
        Task DisconnectAsync();

        Task<bool> InsertAsync(Part part);
        Task<bool> DeleteAsync(int partID);
       
        Task<List<Part>> GetCompatiblePartsAsync(int autoID);
    }
}
