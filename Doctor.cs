using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Doctor
{
    public int Id { get; set; }

    public Person? Person { get; set; }

    public string? Password { get; set; }

    public string? Specialization { get; set; }

    public string? LicenseNumber { get; set; }

    public DateOnly? LicenseExpirationDate { get; set; }
}