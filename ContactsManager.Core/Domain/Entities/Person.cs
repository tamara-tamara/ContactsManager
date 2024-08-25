using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Person
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(40)]
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public Guid? CountryId { get; set; }
        public Country? Country { get; set; }

        public override string ToString()
        {
            return $"{Id} : {Name}";
        }
    }
}
