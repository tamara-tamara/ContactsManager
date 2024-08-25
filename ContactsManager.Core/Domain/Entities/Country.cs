using System.ComponentModel.DataAnnotations;

namespace Entities
{

    /// <summary>
    /// Domain model for storing country's details
    /// </summary>
    public class Country
    {
        [Key]
        public Guid Id { get; set; }

        public string? CountryName { get; set; }
        public ICollection<Person>? Persons { get; set; }

    }
}
