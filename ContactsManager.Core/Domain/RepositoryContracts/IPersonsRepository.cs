using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public interface IPersonsRepository
    {
        Task<Person> AddPerson(Person person);
        Task<Person> UpdatePerson(Person person);
        Task<List<Person>> GetAll();
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);
        Task<Person?> GetPersonById(Guid personId);
        Task<bool> DeletePersonById(Guid personId);

    }
}
