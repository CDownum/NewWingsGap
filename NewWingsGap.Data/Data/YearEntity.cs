using System.ComponentModel.DataAnnotations;

public class YearEntity : Entity
{
    [Key]
    [Required]
    public int Year { get; set; }
}
