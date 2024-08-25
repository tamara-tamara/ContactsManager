using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using ServiceContracts;

namespace Services
{
    public class PersonDeleterService : IPersonDeleterService
    {
        //private readonly ApplicationDbContext _personsRepository;
        private readonly IPersonsRepository _personsRepository;
        //private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonDeleterService(IPersonsRepository personDb, ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personDb;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
           // _countriesService = countriesService;
        }

        //private PersonResponse ConvertPersonToPersonResponse(Person person)
        //{
        //    var response = person.ToPersonResponse();
        //    //  response.Country = (person.CountryId!=null)? ((_countriesService.GetById(person.CountryId))?.CountryName) : null;
        //    response.Country = person.Country?.CountryName;

        //    return response;
        //}

        public async Task<bool> DeletePerson(Guid? id)
        {
            if (id == null) throw new ArgumentNullException("Null ID");

            var person = await _personsRepository.GetPersonById(id.Value);

            if (person == null)
                  return false;

            await _personsRepository.DeletePersonById(id.Value);
           // await _personsRepository.SaveChangesAsync();

            return true;

        }
    }
}
