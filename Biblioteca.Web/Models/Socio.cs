using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models;

public class Socio
{
    public int SocioId { get; set; }

    [Display(Name = "DNI")]
    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 caracteres.")]
    public string DNI { get; set; } = string.Empty;

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(120, ErrorMessage = "El nombre no puede superar los 120 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Display(Name = "Correo")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [StringLength(120, ErrorMessage = "El correo no puede superar los 120 caracteres.")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    public bool Activo { get; set; } = true;
}
