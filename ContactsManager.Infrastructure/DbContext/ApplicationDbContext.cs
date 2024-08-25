using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            //Seed to Countries
            string countriesJson = File.ReadAllText("countries.json");
            List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

            foreach (Country country in countries)
            {
                if (country.Id == Guid.Empty)
                {
                    country.Id = Guid.NewGuid();
                }
                modelBuilder.Entity<Country>().HasData(country);
            }


            //Seed to Persons
            string personsJson = File.ReadAllText("persons.json");
            List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

            foreach (Person person in persons)
            {
                if (person.Id == Guid.Empty)
                {
                    person.Id = Guid.NewGuid();
                }
                modelBuilder.Entity<Person>().HasData(person);
            }

            //Fluent API
            //modelBuilder.Entity<Person>().Property(x => x.Address)
            //    .HasColumnName("HomeAddress")
            //    .HasColumnType("varchar(8)")
            //    .HasDefaultValue("Earth");

            //  modelBuilder.Entity<Person>().HasIndex(person => person.Id).IsUnique();
            //  modelBuilder.Entity<Person>().HasCheckConstraint("CHK_Address", "len([Address]) = 10");

            //Table relations
            modelBuilder.Entity<Person>(person =>
            {
                person.HasOne<Country>(c => c.Country)
                .WithMany(p => p.Persons)
                .HasForeignKey(p => p.CountryId);
            });
        }

        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }

        public int sp_AddPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id",person.Id),
                new SqlParameter("@Name",person.Name),
                new SqlParameter("@Email",person.Email),
                new SqlParameter("@DateOfBirth",person.DateOfBirth),
                new SqlParameter("@Gender",person.Gender),
                new SqlParameter("@CountryId",person.CountryId),
                new SqlParameter("@Address",person.Address),
                new SqlParameter("@ReceiveNewsLetters",person.ReceiveNewsLetters),
            };

           return Database.ExecuteSqlRaw("EXECUTE [dbo].[AddPerson] @Id, @Name, @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLetters", parameters);
        }
    }
}