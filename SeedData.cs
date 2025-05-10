public class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var db = new ClinicContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ClinicContext>>());

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        // if (context == null || context.Person == null)
        // {
        //     throw new NullReferenceException(
        //         "Null ClinicContext or Movie DbSet");
        // }

        // if (context.Person.Any())
        // {
        //     return;
        // }

        // Add Person records first
        db.Person.AddRange(
            new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "555-0123",
                Address = "123 Main St",
                City = "San Francisco",
                State = "CA",
                ZipCode = "94105",
                Country = "USA",
                Gender = "Male",
                DateOfBirth = new DateOnly(1985, 6, 15),
                Notes = "Regular patient, prefers morning appointments"
            },
            new Person
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                Phone = "555-0124",
                Address = "456 Park Avenue",
                City = "New York",
                State = "NY",
                ZipCode = "10022",
                Country = "USA",
                Gender = "Female",
                DateOfBirth = new DateOnly(1992, 3, 28),
                Notes = "Allergic to penicillin, requires special attention"
            },
            new Person
            {
                FirstName = "Dawid",
                LastName = "Kowalski",
                Email = "dawid.kowalski@example.com",
                Phone = "555-0125",
                Address = "789 Lake Shore Drive",
                City = "Chicago",
                State = "IL",
                ZipCode = "60611",
                Country = "USA",
                Gender = "Male",
                DateOfBirth = new DateOnly(1988, 11, 7),
                Notes = "Sports injury follow-up required"
            },
            new Person
            {
                FirstName = "Klaudia",
                LastName = "Kowalska",
                Email = "klaudia.kowalska@example.com",
                Phone = "555-0126",
                Address = "321 Ocean Drive",
                City = "Miami",
                State = "FL",
                ZipCode = "33139",
                Country = "USA",
                Gender = "Female",
                DateOfBirth = new DateOnly(1995, 9, 22),
                Notes = "Regular check-ups every 6 months"
            },
            new Person
            {
                FirstName = "Gustavo",
                LastName = "Gomez",
                Email = "gustavo.gomez@example.com",
                Phone = "555-0127",
                Address = "123 Main St",
                City = "San Francisco",
                State = "CA",
                ZipCode = "94105",
                Country = "USA",
                Gender = "Male",
                DateOfBirth = new DateOnly(1985, 6, 15),
                Notes = "Regular patient, prefers morning appointments"
            },
            new Person
            {
                FirstName = "Dmitry",
                LastName = "Kuznetsov",
                Email = "dmitry.kuznetsov@example.com",
                Phone = "555-0128",
                Address = "123 Main St",
                City = "San Francisco",
                State = "CA",
                ZipCode = "94105",
                Country = "USA",
                Gender = "Male",
                DateOfBirth = new DateOnly(1985, 6, 15),
                Notes = "Regular patient, prefers morning appointments"
            }
        );

        // Save changes to get the IDs
        db.SaveChanges();

        // Now create AdminUser records with the saved Person records
        db.AdminUser.AddRange(
            new AdminUser
            {
                Person = db.Person.First(p => p.Email == "john.doe@example.com"),
                Password = "john.doe123"
            },
            new AdminUser
            {
                Person = db.Person.First(p => p.Email == "jane.doe@example.com"),
                Password = "jane.doe123"
            }
        );

        db.Doctor.AddRange(
            new Doctor
            {
                Person = db.Person.First(p => p.Email == "gustavo.gomez@example.com"),
                Password = "gustavo.gomez123"
            },
            new Doctor
            {
                Person = db.Person.First(p => p.Email == "dmitry.kuznetsov@example.com"),
                Password = "dmitry.kuznetsov123"
            }
        );

        db.Patient.AddRange(
            new Patient
            {
                Person = db.Person.First(p => p.Email == "dawid.kowalski@example.com"),
                InsuranceProvider = "Blue Cross Blue Shield",
                InsuranceNumber = "1234567890",
                InsuranceExpirationDate = new DateOnly(2025, 1, 1)
            },
            new Patient
            {
                Person = db.Person.First(p => p.Email == "gustavo.gomez@example.com"),
                InsuranceProvider = "Blue Cross Blue Shield",
                InsuranceNumber = "1234567890",
                InsuranceExpirationDate = new DateOnly(2025, 1, 1)
            },
            new Patient
            {
                Person = db.Person.First(p => p.Email == "klaudia.kowalska@example.com"),
                InsuranceProvider = "Blue Cross Blue Shield",
                InsuranceNumber = "1234567890",
                InsuranceExpirationDate = new DateOnly(2025, 1, 1)
            }
        );

        db.SaveChanges();

        db.Visit.AddRange(
            new Visit
            {
                Patient = db.Patient.First(p => p.Person!.Email == "dawid.kowalski@example.com"),
                Doctor = db.Doctor.First(d => d.Person!.Email == "gustavo.gomez@example.com"),
                DateTime = new DateTime(2023, 1, 15),
                Reason = "Headache",
                Diagnosis = "Strep throat",
                Treatment = "Antibiotics",
                Notes = "Patient reported a sore throat and headache"
            },
            new Visit
            {
                Patient = db.Patient.First(p => p.Person!.Email == "gustavo.gomez@example.com"),
                Doctor = db.Doctor.First(d => d.Person!.Email == "dmitry.kuznetsov@example.com"),
                DateTime = new DateTime(2023, 1, 15),
                Reason = "Stomach pain",
                Diagnosis = "Gastroenteritis",
                Treatment = "Rest and hydration",
                Notes = "Patient complained of stomach pain and nausea"
            },
            new Visit
            {
                Patient = db.Patient.First(p => p.Person!.Email == "klaudia.kowalska@example.com"),
                Doctor = db.Doctor.First(d => d.Person!.Email == "gustavo.gomez@example.com"),
                DateTime = new DateTime(2023, 1, 15),
                Reason = "Headache",
                Diagnosis = "Strep throat",
                Treatment = "Antibiotics",
                Notes = "Patient reported a sore throat and headache"
            },
            new Visit
            {
                Patient = db.Patient.First(p => p.Person!.Email == "klaudia.kowalska@example.com"),
                Doctor = db.Doctor.First(d => d.Person!.Email == "dmitry.kuznetsov@example.com"),
                DateTime = new DateTime(2023, 1, 15),
                Reason = "Sleepiness",
                Diagnosis = "Sleep apnea",
                Treatment = "CPAP machine",
                Notes = "Patient complained of sleepiness and snoring"
            }
        );

        db.SaveChanges();
    }
}