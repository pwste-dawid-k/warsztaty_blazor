using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Patient
{
    public int Id { get; set; }

    public Person? Person { get; set; }

    public string? InsuranceProvider { get; set; }

    public string? InsuranceNumber { get; set; }

    public DateOnly? InsuranceExpirationDate { get; set; }
}