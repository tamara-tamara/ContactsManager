using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that is used as return type for most of CountryService methods
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }

        /// <summary>
        /// Overriding to test assert data values
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if(obj.GetType() != typeof(CountryResponse)) return false;

            var other = obj as CountryResponse;
            return this.CountryId == other.CountryId && this.CountryName == other.CountryName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryId = country.Id,
                CountryName = country.CountryName
            };
        }
    }
}
