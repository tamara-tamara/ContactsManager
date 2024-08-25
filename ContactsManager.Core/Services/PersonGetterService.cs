using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using SerilogTimings;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Globalization;

namespace Services
{
    public class PersonGetterService : IPersonGetterService
    {
        //private readonly ApplicationDbContext _personsRepository;
        private readonly IPersonsRepository _personsRepository;
        //private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonGetterService(IPersonsRepository personDb, ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personDb;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
           // _countriesService = countriesService;
        }

        //private PersonResponse ConvertPersonToPersonResponse(Person person)
        //{
        //    var response = person.ToPersonResponse();
        //    //  response.Country = (person.CountryId!=null)? ((_countriesService.GetById(person.CountryId))?.CountryName) : null;
        //    response.Country = person.Country?.CountryName;

        //    return response;
        //}

        public async Task<List<PersonResponse>> GetAllPerson()
        {
            _logger.LogInformation("GetAllPersons of PersonService");
            //var persons = await _personsRepository.Persons.Include("Country").ToListAsync();            // return _db.Persons.Select(p => ConvertPersonToPersonResponse(p)).ToList();
            var persons = await _personsRepository.GetAll();            // return _db.Persons.Select(p => ConvertPersonToPersonResponse(p)).ToList();
              return persons.Select(p => p.ToPersonResponse()).ToList();

           // return _db.sp_GetAllPersons().Select(p => ConvertPersonToPersonResponse(p)).ToList(); //using stored procedure
        }

        public async Task<PersonResponse?> GetById(Guid? id)
        {
            if (id == null)
            {
                return null;
            }

            Person? person = await _personsRepository.GetPersonById( id.Value);
            if (person == null)
            {
                return null;
            }

            return person.ToPersonResponse();  //now we can use this method instead of ConvertPersonToPersonResponse since we modified ToPersonResonce method with navigations
        }

        public async Task<List<PersonResponse>> GetFilteredPerson(string searchBy, string? searchString)
        {
            _logger.LogInformation("Getfilteredpersons ");
            List<Person> persons = null;

            using (Operation.Time("Time for FilteredPeople of PersonService"))
            {
                persons = searchBy switch
                {
                    //var allPeople = await GetAllPerson();
                    //if(searchBy == null || searchString == null)
                    //{
                    //    return allPeople;
                    //}
                    //var matchingPeople = allPeople;
                    //switch(searchBy)
                    //{
                    nameof(PersonResponse.Name) =>
                       await _personsRepository.GetFilteredPersons(p => p.Name.Contains(searchString)),

                    nameof(PersonResponse.Email) =>
                         await _personsRepository.GetFilteredPersons(p => p.Email.Contains(searchString)),

                    nameof(PersonResponse.DateOfBirth) =>
                         await _personsRepository.GetFilteredPersons(p => p.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString)),

                    nameof(PersonResponse.Gender) =>
                         await _personsRepository.GetFilteredPersons(p => p.Gender.Contains(searchString)),

                    nameof(PersonResponse.CountryId) =>
                         await _personsRepository.GetFilteredPersons(p => p.Country.CountryName.Contains(searchString)),

                    nameof(PersonResponse.Address) =>
                         await _personsRepository.GetFilteredPersons(p => p.Address.Contains(searchString)),

                    _ => await _personsRepository.GetAll()
                };
            }
            _diagnosticContext.Set("Persons", persons);

            return persons.Select(x => x.ToPersonResponse()).ToList();

        }

        public async Task<MemoryStream> GetPersonCSV()
        {
            //MemoryStream memoryStream = new MemoryStream();
            //StreamWriter streamWriter = new StreamWriter(memoryStream);
            //CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);

            //csvWriter.WriteHeader<PersonResponse>();
            //await csvWriter.NextRecordAsync();
            //var persons = await _db.Persons.Include("Country").Select(p => p.ToPersonResponse()).ToListAsync();
            //await csvWriter.WriteRecordsAsync(persons);

            //memoryStream.Position = 0;

            //another approach
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
            CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration);


            csvWriter.WriteField(nameof(PersonResponse.Name));
            csvWriter.WriteField(nameof(PersonResponse.Email));
            csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
            csvWriter.WriteField(nameof(PersonResponse.Age));
            csvWriter.WriteField(nameof(PersonResponse.Country));
            csvWriter.WriteField(nameof(PersonResponse.Address));
            csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));

            await csvWriter.NextRecordAsync();
            var persons = await GetAllPerson();

            foreach(var person in persons)
            {
                csvWriter.WriteField(person.Name);
                csvWriter.WriteField(person.Email);
                if (person.DateOfBirth.HasValue)
                    csvWriter.WriteField(person.DateOfBirth.Value.ToString("yyyy-MM-dd"));
                else
                    csvWriter.WriteField("");

                csvWriter.WriteField(person.Age);
                csvWriter.WriteField(person.Country);
                csvWriter.WriteField(person.Address);
                csvWriter.WriteField(person.ReceiveNewsLetters);

                await csvWriter.NextRecordAsync();
                await csvWriter.FlushAsync();
            }

            memoryStream.Position = 0;

            return memoryStream;
        }

        public async Task<MemoryStream> GetPersonExcel()
        {
           MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets
                    .Add("PersonsSheet");
                excelWorksheet.Cells["A1"].Value = "Person Name";
                excelWorksheet.Cells["B1"].Value = "Email";
                excelWorksheet.Cells["C1"].Value = "Date of Birth";
                excelWorksheet.Cells["D1"].Value = "Age";
                excelWorksheet.Cells["E1"].Value = "Gender";
                excelWorksheet.Cells["F1"].Value = "Country";
                excelWorksheet.Cells["G1"].Value = "Address";
                excelWorksheet.Cells["H1"].Value = "Receive News Letters";

                using (ExcelRange headerCells = excelWorksheet.Cells["A1:H1"])
                {
                    headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Aqua);
                    headerCells.Style.Font.Bold = true;
                }

                int row = 2;
                var persons = await GetAllPerson();

                foreach (var person in persons)
                {
                    excelWorksheet.Cells[row, 1].Value = person.Name;
                    excelWorksheet.Cells[row, 2].Value = person.Email;
                    if (person.DateOfBirth.HasValue)
                        excelWorksheet.Cells[row, 3].Value = person.DateOfBirth.Value.ToString("yyyy-MM-dd");
                    else
                        excelWorksheet.Cells[row, 3].Value = "";

                    excelWorksheet.Cells[row, 4].Value = person.Age;
                    excelWorksheet.Cells[row, 5].Value = person.Gender;
                    excelWorksheet.Cells[row, 6].Value = person.Country;
                    excelWorksheet.Cells[row, 7].Value = person.Address;
                    excelWorksheet.Cells[row, 8].Value = person.ReceiveNewsLetters;

                    row++;
                }
                excelWorksheet.Cells[$"A1:H{row}"].AutoFitColumns();
                await excelPackage.SaveAsync();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
