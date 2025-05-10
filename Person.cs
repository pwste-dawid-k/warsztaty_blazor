using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Person
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [Phone]
    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? ZipCode { get; set; }

    public string? Country { get; set; }

    public string? Gender { get; set; }

    [DataType(DataType.Date)]
    public DateOnly? DateOfBirth { get; set; }

    public string? Notes { get; set; }
}