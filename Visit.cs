using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Visit
{
    public int Id { get; set; }

    public Patient? Patient { get; set; }

    public Doctor? Doctor { get; set; }

    public DateTime? DateTime { get; set; }

    public string? Reason { get; set; }

    public string? Diagnosis { get; set; }

    public string? Treatment { get; set; }

    public string? Notes { get; set; }
}