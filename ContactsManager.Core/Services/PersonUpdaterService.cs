using Exceptions;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class PersonUpdaterService : IPersonUpdaterService
    {
        //private readonly ApplicationDbContext _personsRepository;
        private readonly IPersonsRepository _personsRepository;
        //private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonUpdaterService(IPersonsRepository personDb, ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext)
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

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            ValidationHelper.ModelValidation(request);

           var person = await _personsRepository.GetPersonById(request.Id);
            if (person == null)
                throw new InvalidPersonIdException("Person doesnt exist.");

            person.Name = request.Name;
            person.Address = request.Address;
            person.Gender = request.Gender.ToString();
            person.ReceiveNewsLetters = request.ReceiveNewsLetters;
            person.CountryId = request.CountryId;
            person.DateOfBirth = request.DateOfBirth;
            person.Email = request.Email;

            await _personsRepository.UpdatePerson(person);

            return person.ToPersonResponse();
        }
    }
}
