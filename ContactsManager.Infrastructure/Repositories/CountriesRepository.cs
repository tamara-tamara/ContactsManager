using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly ApplicationDbContext _context;
        public CountriesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Country> AddCountry(Country country)
        {
           _context.Countries.Add(country);
           await _context.SaveChangesAsync();
            return country;
        }

        public async Task<List<Country>> GetAll()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<Country?> GetCountryById(Guid countryId)
        {
            return await _context.Countries.FirstOrDefaultAsync(x => x.Id == countryId);
        }

        public async Task<Country?> GetCountryByName(string name)
        {
            return await _context.Countries.FirstOrDefaultAsync(x => x.CountryName == name);
        }
    }
}
