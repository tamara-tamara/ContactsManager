using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public Guid? CountryId { get; set; }
        public string? Country { get; set; }
        public double? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse)) return false;

            var other = obj as PersonResponse;

            return other.Id == this.Id && this.Name == other.Name && this.Email == other.Email;
        }
        public override string ToString()
        {
            return $"{Id}, {Name}, {Email}";
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                Id = Id,
                Name =Name,
                Email = Email,
                Gender = (GenderEnum)Enum.Parse(typeof(GenderEnum),Gender,true),
                DateOfBirth = DateOfBirth,
                Address = Address,
                CountryId = CountryId,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
    }

    public static class PersonExtension
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse { Id = person.Id, Name = person.Name, Email = person.Email, Gender = person.Gender, DateOfBirth = person.DateOfBirth,
                Address = person.Address,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null, CountryId = person.CountryId, ReceiveNewsLetters = person.ReceiveNewsLetters,
                Country = person.Country?.CountryName
            };

        }


    }
}
