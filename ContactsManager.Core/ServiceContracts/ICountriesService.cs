using ServiceContracts.DTO;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRrequest"></param>
        /// <returns>Country object after adding it</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRrequest);
        
        /// <summary>
        /// Returns all countries
        /// </summary>
        /// <returns></returns>
        Task<List<CountryResponse>> GetCountryList();

        /// <summary>
        /// Returns country by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CountryResponse?> GetById(Guid? id);

        Task<int> UploadCountriesFromExcelFiles(IFormFile formFile);

    }
}
