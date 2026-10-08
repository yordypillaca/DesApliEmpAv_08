using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models;

public class PrestamoReporte
{
    public int PrestamoId { get; set; }

    [Display(Name = "Socio")]
    public string Socio { get; set; } = string.Empty;

    [Display(Name = "Libro")]
    public string Libro { get; set; } = string.Empty;

    [Display(Name = "Fecha de préstamo")]
    [DataType(DataType.Date)]
    public DateTime FechaPrestamo { get; set; }

    [Display(Name = "Fecha límite")]
    [DataType(DataType.Date)]
    public DateTime FechaLimite { get; set; }

    [Display(Name = "Estado")]
    public string Estado { get; set; } = string.Empty;

    [Display(Name = "Fecha de devolución")]
    [DataType(DataType.Date)]
    public DateTime? FechaDevolucion { get; set; }
}
