using Microsoft.Data.Sqlite;
using Microsoft.Extensions.ObjectPool;

public class Database
{
    private static Database? _instance;
    // private readonly string _connectionString = "Data Source=database.db";
    private readonly string _connectionString = "Data Source=:memory:";
    private readonly SqliteConnection _connection;

    private Database()
    {
        _connection = new SqliteConnection(_connectionString);
        _connection.Open();
        CreateTables();
        FillDatabase();
    }

    public static Database GetInstance()
    {
        // print current dir
        Console.WriteLine(Directory.GetCurrentDirectory());
        _instance ??= new Database();
        return _instance;
    }

    public long TableCount()
    {
        // using var conn = new SqliteConnection(_connectionString);
        // conn.Open();
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            SELECT COUNT(*) FROM sqlite_master WHERE type='table';
        ";
        return (long)(cmd.ExecuteScalar() ?? throw new InvalidOperationException("Query returned no results"));
    }

    public bool IsDatabaseInitialized()
    {
        try
        {
            return TableCount() > 0;
        }
        catch
        {
            return false;
        }
    }

    // public void DeleteDatabase()
    // {
    // }

    public class Person
    {
        public required int PersonId { get; set; }
        public required string PersonName { get; set; }
        public required int PersonAge { get; set; }
        public required string PersonGender { get; set; }
        public required string PersonPhone { get; set; }
        public required string PersonEmail { get; set; }
        public required string PersonAddress { get; set; }
        public required string PersonCity { get; set; }
        public required string PersonState { get; set; }
        public required string PersonZip { get; set; }
        public required DateTime PersonCreatedAt { get; set; }
        public required DateTime PersonUpdatedAt { get; set; }
    }

    public class Patient : Person
    {
        public required int PatientId { get; set; }
        public required string PatientPassword { get; set; }
    }

    public class Doctor : Person
    {
        public required int DoctorId { get; set; }
        public required string DoctorPassword { get; set; }
    }

    public class Admin : Person
    {
        public required int AdminId { get; set; }
        public required string AdminPassword { get; set; }
    }

    public class Visit
    {
        public required int VisitId { get; set; }
        public required int VisitPatientId { get; set; }
        public required int VisitDoctorId { get; set; }
        public required string VisitType { get; set; }
        public required string VisitNotes { get; set; }
        public required string VisitStatus { get; set; }
        public required DateTime VisitDate { get; set; }
        public required DateTime VisitCreatedAt { get; set; }
        public required DateTime VisitUpdatedAt { get; set; }
        public required DateTime VisitDateTime { get; set; }
    }

    public Dictionary<int, Patient> GetPatients()
    {
        // using var conn = new SqliteConnection(_connectionString);
        // conn.Open();
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            SELECT p.*, pt.* 
            FROM person p 
            JOIN patient pt ON p.person_id = pt.patient_person_id;
        ";
        var result = new Dictionary<int, Patient>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var patient = new Patient
            {
                PatientPassword = reader.GetString(reader.GetOrdinal("patient_password")),
                PatientId = reader.GetInt32(reader.GetOrdinal("patient_id")),
                PersonId = reader.GetInt32(reader.GetOrdinal("patient_person_id")),
                PersonName = reader.GetString(reader.GetOrdinal("person_name")),
                PersonAge = reader.GetInt32(reader.GetOrdinal("person_age")),
                PersonGender = reader.GetString(reader.GetOrdinal("person_gender")),
                PersonPhone = reader.GetString(reader.GetOrdinal("person_phone")),
                PersonEmail = reader.GetString(reader.GetOrdinal("person_email")),
                PersonAddress = reader.GetString(reader.GetOrdinal("person_address")),
                PersonCity = reader.GetString(reader.GetOrdinal("person_city")),
                PersonState = reader.GetString(reader.GetOrdinal("person_state")),
                PersonZip = reader.GetString(reader.GetOrdinal("person_zip")),
                PersonCreatedAt = DateTime.Parse(reader.GetString(reader.GetOrdinal("person_created_at"))),
                PersonUpdatedAt = DateTime.Parse(reader.GetString(reader.GetOrdinal("person_updated_at")))
            };
            result.Add(patient.PatientId, patient);
        }
        return result;
    }

    public Dictionary<int, Doctor> GetDoctors()
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            SELECT p.*, d.* 
            FROM person p 
            JOIN doctor d ON p.person_id = d.doctor_person_id;
        ";
        var result = new Dictionary<int, Doctor>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var doctor = new Doctor
            {
                DoctorId = reader.GetInt32(reader.GetOrdinal("doctor_id")),
                DoctorPassword = reader.GetString(reader.GetOrdinal("doctor_password")),
                PersonId = reader.GetInt32(reader.GetOrdinal("doctor_person_id")),
                PersonName = reader.GetString(reader.GetOrdinal("person_name")),
                PersonAge = reader.GetInt32(reader.GetOrdinal("person_age")),
                PersonGender = reader.GetString(reader.GetOrdinal("person_gender")),
                PersonPhone = reader.GetString(reader.GetOrdinal("person_phone")),
                PersonEmail = reader.GetString(reader.GetOrdinal("person_email")),
                PersonAddress = reader.GetString(reader.GetOrdinal("person_address")),
                PersonCity = reader.GetString(reader.GetOrdinal("person_city")),
                PersonState = reader.GetString(reader.GetOrdinal("person_state")),
                PersonZip = reader.GetString(reader.GetOrdinal("person_zip")),
                PersonCreatedAt = DateTime.Parse(reader.GetString(reader.GetOrdinal("person_created_at"))),
                PersonUpdatedAt = DateTime.Parse(reader.GetString(reader.GetOrdinal("person_updated_at")))
            };
            result.Add(doctor.DoctorId, doctor);
        }
        return result;
    }

    public Dictionary<int, Visit> GetVisits()
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            SELECT v.*, pp.person_name AS patient_name, pd.person_name AS doctor_name
            FROM visit v
            JOIN patient p ON v.visit_patient_id = p.patient_id
            JOIN person pp ON p.patient_person_id = pp.person_id
            JOIN doctor d ON v.visit_doctor_id = d.doctor_id
            JOIN person pd ON d.doctor_person_id = pd.person_id;
        ";
        var result = new Dictionary<int, Visit>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var visit = new Visit
            {
                VisitDateTime = DateTime.Parse(reader.GetString(reader.GetOrdinal("visit_datetime"))),
                VisitId = reader.GetInt32(reader.GetOrdinal("visit_id")),
                VisitPatientId = reader.GetInt32(reader.GetOrdinal("visit_patient_id")),
                VisitDoctorId = reader.GetInt32(reader.GetOrdinal("visit_doctor_id")),
                VisitDate = DateTime.Parse(reader.GetString(reader.GetOrdinal("visit_date"))),
                VisitType = reader.GetString(reader.GetOrdinal("visit_type")),
                VisitNotes = reader.GetString(reader.GetOrdinal("visit_notes")),
                VisitStatus = reader.GetString(reader.GetOrdinal("visit_status")),
                VisitCreatedAt = DateTime.Parse(reader.GetString(reader.GetOrdinal("visit_created_at"))),
                VisitUpdatedAt = DateTime.Parse(reader.GetString(reader.GetOrdinal("visit_updated_at")))
            };
            result.Add(visit.VisitId, visit);
        }
        return result;
    }

    public void CreateTables()
    {
        // using var conn = new SqliteConnection(_connectionString);
        // conn.Open();
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS person (
                person_id INTEGER PRIMARY KEY AUTOINCREMENT,
                person_name TEXT NOT NULL,
                person_age INTEGER NOT NULL,
                person_gender TEXT NOT NULL,
                person_phone TEXT NOT NULL,
                person_email TEXT NOT NULL,
                person_address TEXT NOT NULL,
                person_city TEXT NOT NULL,
                person_state TEXT NOT NULL,
                person_zip TEXT NOT NULL,
                person_created_at TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                person_updated_at TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
            ) STRICT;

            CREATE TABLE IF NOT EXISTS patient (
                patient_id INTEGER PRIMARY KEY AUTOINCREMENT,
                patient_person_id INTEGER NOT NULL,
                patient_password TEXT NOT NULL,
                FOREIGN KEY (patient_person_id) REFERENCES person(person_id)
            ) STRICT;
            
            CREATE TABLE IF NOT EXISTS doctor (
                doctor_id INTEGER PRIMARY KEY AUTOINCREMENT,
                doctor_person_id INTEGER NOT NULL,
                doctor_password TEXT NOT NULL,
                FOREIGN KEY (doctor_person_id) REFERENCES person(person_id)
            ) STRICT;

            CREATE TABLE IF NOT EXISTS admin (
                admin_id INTEGER PRIMARY KEY AUTOINCREMENT,
                admin_person_id INTEGER NOT NULL,
                admin_password TEXT NOT NULL,
                FOREIGN KEY (admin_person_id) REFERENCES person(person_id)
            ) STRICT;

            CREATE TABLE IF NOT EXISTS visit (
                visit_id INTEGER PRIMARY KEY AUTOINCREMENT,
                visit_patient_id INTEGER NOT NULL,
                visit_doctor_id INTEGER NOT NULL,
                visit_date TEXT NOT NULL,
                visit_type TEXT NOT NULL,
                visit_notes TEXT NOT NULL,
                visit_status TEXT NOT NULL,
                visit_datetime TEXT NOT NULL,
                visit_created_at TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                visit_updated_at TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (visit_patient_id) REFERENCES patient(patient_id),
                FOREIGN KEY (visit_doctor_id) REFERENCES doctor(doctor_id)
            ) STRICT;

            CREATE TRIGGER update_person_timestamp
                AFTER UPDATE ON person
                FOR EACH ROW
            BEGIN
                UPDATE person 
                SET person_updated_at = CURRENT_TIMESTAMP
                WHERE person_id = NEW.person_id;
            END;

            CREATE TRIGGER update_visit_timestamp
                AFTER UPDATE ON visit
                FOR EACH ROW
            BEGIN
                UPDATE visit 
                SET visit_updated_at = CURRENT_TIMESTAMP
                WHERE visit_id = NEW.visit_id;
            END;
        ";
        cmd.ExecuteNonQuery();
    }

    public long CreatePerson(string name, int age, string gender, string phone, string email, string address, string city, string state, string zip)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO person (person_name, person_age, person_gender, person_phone, person_email, person_address, person_city, person_state, person_zip)
            VALUES (@name, @age, @gender, @phone, @email, @address, @city, @state, @zip);
            SELECT last_insert_rowid();
        ";
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@age", age);
        cmd.Parameters.AddWithValue("@gender", gender);
        cmd.Parameters.AddWithValue("@phone", phone);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@address", address);
        cmd.Parameters.AddWithValue("@city", city);
        cmd.Parameters.AddWithValue("@state", state);
        cmd.Parameters.AddWithValue("@zip", zip);
        var personId = (long)(cmd.ExecuteScalar() ?? throw new InvalidOperationException("Query returned no results"));
        return personId;
    }

    public int CreatePatient(long PersonId, string password)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO patient (patient_person_id, patient_password)
            VALUES (@person_id, @password);
            SELECT last_insert_rowid();
        ";
        cmd.Parameters.AddWithValue("@person_id", PersonId);
        cmd.Parameters.AddWithValue("@password", password);
        var patientId = (long)(cmd.ExecuteScalar() ?? throw new InvalidOperationException("Query returned no results"));
        return (int)patientId;
    }

    public int CreateDoctor(long PersonId, string password)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO doctor (doctor_person_id, doctor_password)
            VALUES (@person_id, @password);
            SELECT last_insert_rowid();
        ";
        cmd.Parameters.AddWithValue("@person_id", PersonId);
        cmd.Parameters.AddWithValue("@password", password);
        var doctorId = (long)(cmd.ExecuteScalar() ?? throw new InvalidOperationException("Query returned no results"));
        return (int)doctorId;
    }

    public int CreateAdmin(long PersonId, string password)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO admin (admin_person_id, admin_password)
            VALUES (@person_id, @password);
            SELECT last_insert_rowid();
        ";
        cmd.Parameters.AddWithValue("@person_id", PersonId);
        cmd.Parameters.AddWithValue("@password", password);
        var adminId = (long)(cmd.ExecuteScalar() ?? throw new InvalidOperationException("Query returned no results"));
        return (int)adminId;
    }

    public void CreateVisit(int patient_id, int doctor_id, DateTime date, DateTime time, string type, string notes, string status)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO visit (visit_patient_id, visit_doctor_id, visit_date, visit_datetime, visit_type, visit_notes, visit_status)
            VALUES (@patient_id, @doctor_id, @date, @time, @type, @notes, @status);
        ";
        cmd.Parameters.AddWithValue("@patient_id", patient_id);
        cmd.Parameters.AddWithValue("@doctor_id", doctor_id);
        cmd.Parameters.AddWithValue("@date", date);
        cmd.Parameters.AddWithValue("@time", time);
        cmd.Parameters.AddWithValue("@type", type);
        cmd.Parameters.AddWithValue("@notes", notes);
        cmd.Parameters.AddWithValue("@status", status);
        var rowsAffected = cmd.ExecuteNonQuery();
        if (rowsAffected != 1)
        {
            throw new InvalidOperationException($"Expected 1 row to be affected, but {rowsAffected} rows were affected.");
        }
    }

    public void Dump()
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            SELECT sql FROM sqlite_master
            WHERE sql IS NOT NULL
            ORDER BY type = 'table' DESC, name;
        ";
        var result = cmd.ExecuteScalar();
        Console.WriteLine("!!! DUMP !!!");
        Console.WriteLine(result);
    }

    public void FillDatabase()
    {
        var p1 = CreatePerson("John Doe", 30, "Male", "1234567890", "john.doe@example.com", "123 Main St", "Anytown", "CA", "12345");
        var p2 = CreatePerson("Jane Doe", 25, "Female", "1234567890", "jane.doe@example.com", "123 Main St", "Anytown", "CA", "12345");
        var p3 = CreatePerson("Jim Doe", 40, "Male", "1234567890", "jim.doe@example.com", "123 Main St", "Anytown", "CA", "12345");
        var p4 = CreatePerson("Jill Doe", 35, "Female", "1234567890", "jill.doe@example.com", "123 Main St", "Anytown", "CA", "12345");
        var p5 = CreatePerson("Dr. John Doe", 30, "Male", "1234567890", "john.doe@example.com", "123 Main St", "Anytown", "CA", "12345");
        var p6 = CreatePerson("Admin User", 35, "Male", "1234567890", "admin@example.com", "123 Main St", "Anytown", "CA", "12345");

        var patient_1 = CreatePatient(p1, "password1");
        var patient_2 = CreatePatient(p2, "password2");

        var doctor_1 = CreateDoctor(p3, "password3");
        var doctor_2 = CreateDoctor(p4, "password4");
        var doctor_3 = CreateDoctor(p5, "password5");

        var admin_1 = CreateAdmin(p6, "admin123");

        CreateVisit(patient_1, doctor_1, DateTime.Now, DateTime.Now, "Consultation", "Patient has a cold", "Completed");
        CreateVisit(patient_2, doctor_2, DateTime.Now, DateTime.Now, "Checkup", "Patient is healthy", "Completed");
        CreateVisit(patient_1, doctor_3, DateTime.Now, DateTime.Now, "Consultation", "Patient has a headache", "Completed");
        CreateVisit(patient_2, doctor_3, DateTime.Now, DateTime.Now, "Checkup", "Patient is healthy", "Completed");
        CreateVisit(patient_1, doctor_1, DateTime.Now, DateTime.Now, "Consultation", "Patient has a cold", "Completed");
    }

    // public Admin? AuthenticateAdmin(string email, string password)
    // {
    //     var cmd = _connection.CreateCommand();
    //     cmd.CommandText = @"
    //         SELECT p.*, a.* 
    //         FROM person p 
    //         JOIN admin a ON p.person_id = a.admin_person_id
    //         WHERE p.person_email = @email AND a.admin_password = @password;
    //     ";
    //     cmd.Parameters.AddWithValue("@email", email);
    //     cmd.Parameters.AddWithValue("@password", password);

    //     using var reader = cmd.ExecuteReader();
    //     if (!reader.Read())
    //     {
    //         return null;
    //     }
    //     else
    //     {
    //         return new Admin
    //         {
    //             AdminId = reader.GetInt32(reader.GetOrdinal("admin_id")),
    //             AdminPassword = reader.GetString(reader.GetOrdinal("admin_password")),
    //             PersonId = reader.GetInt32(reader.GetOrdinal("admin_person_id")),
    //             PersonName = reader.GetString(reader.GetOrdinal("person_name")),
    //             PersonAge = reader.GetInt32(reader.GetOrdinal("person_age")),
    //             PersonGender = reader.GetString(reader.GetOrdinal("person_gender")),
    //             PersonPhone = reader.GetString(reader.GetOrdinal("person_phone")),
    //             PersonEmail = reader.GetString(reader.GetOrdinal("person_email")),
    //             PersonAddress = reader.GetString(reader.GetOrdinal("person_address")),
    //             PersonCity = reader.GetString(reader.GetOrdinal("person_city")),
    //             PersonState = reader.GetString(reader.GetOrdinal("person_state")),
    //             PersonZip = reader.GetString(reader.GetOrdinal("person_zip")),
    //             PersonCreatedAt = DateTime.Parse(reader.GetString(reader.GetOrdinal("person_created_at"))),
    //             PersonUpdatedAt = DateTime.Parse(reader.GetString(reader.GetOrdinal("person_updated_at")))
    //         };
    //     }
    // }
}