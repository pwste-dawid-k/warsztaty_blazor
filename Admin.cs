using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AdminUser
{
    public int Id { get; set; }

    public Person? Person { get; set; }

    public string? Password { get; set; }

}