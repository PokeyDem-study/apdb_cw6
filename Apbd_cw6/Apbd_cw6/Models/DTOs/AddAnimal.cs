using System.ComponentModel.DataAnnotations;

namespace Apbd_cw6.Models.DTOs;

public class AddAnimal
{
    [Required] //Validation
    [MinLength(3)]
    [MaxLength(200)]
    public string Name { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
}