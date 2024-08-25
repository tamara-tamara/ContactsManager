using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class PersonAdderService : IPersonAdderService
    {
        //private readonly ApplicationDbContext _personsRepository;
        private readonly IPersonsRepository _personsRepository;
        //private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonAdderService(IPersonsRepository personDb, ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext)
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
        public async Task<PersonResponse> AddPerson(PersonAddRequest? request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            ValidationHelper.ModelValidation(request);

            var person = request.ToPerson();
            person.Id = Guid.NewGuid();
            await _personsRepository.AddPerson(person);
            //await _personsRepository.SaveChangesAsync();

            //_db.sp_AddPerson(person);  //using procedure


            return person.ToPersonResponse();
        }
    }
}
