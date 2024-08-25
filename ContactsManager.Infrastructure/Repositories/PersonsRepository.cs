using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PersonsRepository> _logger;
        public PersonsRepository(ApplicationDbContext context, ILogger<PersonsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Person> AddPerson(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePersonById(Guid personId)
        {
            _context.Persons.RemoveRange(_context.Persons.Where(x => x.Id == personId));
            int numOfRows = await _context.SaveChangesAsync();
            return numOfRows > 0;
        }

        public async Task<List<Person>> GetAll()
        {
          return await _context.Persons.Include("Country").ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            _logger.LogInformation("getFilteredPersons of Person Repo");
           return await _context.Persons.Include("Country").Where(predicate).ToListAsync();
        }

        public async Task<Person?> GetPersonById(Guid personId)
        {
            return await _context.Persons.Include("Country").FirstOrDefaultAsync(x => x.Id == personId);
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            var matchingPerson =await _context.Persons.FirstOrDefaultAsync(x => x.Id == person.Id);

            if (matchingPerson == null)
                return person;

            matchingPerson.Name = person.Name;
            matchingPerson.Email = person.Email;
            matchingPerson.DateOfBirth = person.DateOfBirth;
            matchingPerson.Gender = person.Gender;
            matchingPerson.CountryId = person.CountryId;
            matchingPerson.Address = person.Address;
            matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;

            await _context.SaveChangesAsync();
            return matchingPerson;
        }
    }
}
