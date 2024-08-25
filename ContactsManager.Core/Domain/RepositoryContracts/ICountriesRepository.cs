using Entities;

namespace RepositoryContracts
{
    public interface ICountriesRepository
    {
        Task<Country> AddCountry(Country country);
        Task<List<Country>> GetAll();
        Task<Country?> GetCountryById(Guid countryId);
        Task<Country?> GetCountryByName(string name);
    }
}
