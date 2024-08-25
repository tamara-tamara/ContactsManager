using Entities;
using OfficeOpenXml;
using ServiceContracts;
using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;
using RepositoryContracts;


namespace Services
{
    public class CountriesService : ICountriesService
    {
        //private readonly ApplicationDbContext _countriesRepository;
        private readonly ICountriesRepository _countriesRepository;

        public CountriesService(ICountriesRepository personDbContext)
        {
            _countriesRepository = personDbContext;
        }
        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException("Null");
            }

            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException("Null");
            }

            if (_countriesRepository.GetCountryByName(countryAddRequest.CountryName) != null)
            {
                throw new ArgumentException("Duplicate country name");
            }

            Country country = countryAddRequest.ToCountry();
            country.Id = Guid.NewGuid();
            await _countriesRepository.AddCountry(country);
           // await _countriesRepository.SaveChangesAsync();  save changes are happining in repo

            return country.ToCountryResponse();
        }

        public async Task<CountryResponse?> GetById(Guid? id)
        {
            if (id == null)
            {
                return null;
            }
            var response = await _countriesRepository.GetCountryById(id.Value);
            return response?.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetCountryList()
        {
           return  (await _countriesRepository.GetAll()).Select(country => country.ToCountryResponse()).ToList();
        }

        public async Task<int> UploadCountriesFromExcelFiles(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            formFile.CopyToAsync(memoryStream);
            int numOfInsertion = 0;

            using (ExcelPackage package = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets["Countries"];
                int row = workSheet.Dimension.Rows;

                for (int i = 2; i <= row; i++)
                {
                   var cellValue = Convert.ToString(workSheet.Cells[i, 1].Value);

                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        var name = cellValue;

                        if (await _countriesRepository.GetCountryByName(name) == null)
                        {
                            var country = new Country()
                            {
                                CountryName = name
                            };
                            await _countriesRepository.AddCountry(country);
                            //await _countriesRepository.SaveChangesAsync();

                            numOfInsertion++;
                        }
                    } 
                }
            }
            return numOfInsertion;
        }
    }
}
