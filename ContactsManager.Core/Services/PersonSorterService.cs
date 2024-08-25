using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace Services
{
    public class PersonSorterService : IPersonSorterService
    {
        //private readonly ApplicationDbContext _personsRepository;
        private readonly IPersonsRepository _personsRepository;
        //private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonSorterService(IPersonsRepository personDb, ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext)
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
   
        public async Task<List<PersonResponse>> GetSortedPerson(List<PersonResponse> allPeople, string sortBy, SortOrderEnum sortOrder)
        {
            if (sortBy == null) return allPeople;

            var sortedPeople = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.Name), SortOrderEnum.ASC)
                     => allPeople.OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Name), SortOrderEnum.DESC)
                    => allPeople.OrderByDescending(p => p.Name, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderEnum.ASC)
                 => allPeople.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderEnum.DESC)
                    => allPeople.OrderByDescending(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderEnum.ASC)
                    => allPeople.OrderBy(p => p.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderEnum.DESC)
                    => allPeople.OrderByDescending(p => p.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderEnum.ASC)
                    => allPeople.OrderBy(p => p.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderEnum.DESC)
                    => allPeople.OrderByDescending(p => p.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderEnum.ASC)
                     => allPeople.OrderBy(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Gender), SortOrderEnum.DESC)
                    => allPeople.OrderByDescending(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderEnum.ASC)
                    => allPeople.OrderBy(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderEnum.DESC)
                    => allPeople.OrderByDescending(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderEnum.ASC)
                    => allPeople.OrderBy(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderEnum.DESC)
                    => allPeople.OrderByDescending(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderEnum.ASC)
                    => allPeople.OrderBy(p => p.ReceiveNewsLetters).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderEnum.DESC)
                    => allPeople.OrderByDescending(p => p.ReceiveNewsLetters).ToList(),

                _ => allPeople
            };
            return sortedPeople;
        }
    }
}
