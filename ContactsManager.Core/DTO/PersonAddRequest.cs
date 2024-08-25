using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        [Required(ErrorMessage ="Name cannot be blank")]
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
