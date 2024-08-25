using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IPersonGetterService
    {
        Task<List<PersonResponse>> GetAllPerson();
        Task<PersonResponse?> GetById(Guid? id);
        Task<List<PersonResponse>> GetFilteredPerson(string searchBy, string? searchString);
        Task<MemoryStream> GetPersonCSV();
        Task<MemoryStream> GetPersonExcel();

    }
}
