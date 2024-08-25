using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PersonUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Name cannot be blank")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Email cannot be blank")]
        [EmailAddress]
        public string? Email { get; set; }
        public GenderEnum? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public Guid? CountryId { get; set; }

        public Person ToPerson()
        {
            return new Person
            {
                Id = Id,
                Name = Name,
                Email = Email,
                Gender = Gender.ToString(),
                Address = Address,
                CountryId = CountryId,
                DateOfBirth = (DateTime)DateOfBirth,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
}
}
