using System.ComponentModel.DataAnnotations;

namespace privaxnet_api.Dtos;


public class UserDto
{
    public string Name { get; set; }

    [Required(ErrorMessage = "O contacto deve ser preenchido")]
    [Phone(ErrorMessage = "Contacto invalido")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "O Email deve ser preenchido")]
    [EmailAddress(ErrorMessage = "Email invalido")]
    public string Email { get; set; }
    public string Password { get; set; }

}
