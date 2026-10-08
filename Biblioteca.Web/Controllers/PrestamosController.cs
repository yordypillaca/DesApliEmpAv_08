using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers;

public class PrestamosController : Controller
{
    private readonly SocioRepositorio _socioRepositorio;

    public PrestamosController(SocioRepositorio socioRepositorio)
    {
        _socioRepositorio = socioRepositorio;
    }

    public async Task<IActionResult> Reporte(DateTime? desde, DateTime? hasta)
    {
        ViewData["Title"] = "Reporte de préstamos";
        ViewData["Desde"] = desde?.ToString("yyyy-MM-dd") ?? string.Empty;
        ViewData["Hasta"] = hasta?.ToString("yyyy-MM-dd") ?? string.Empty;

        var filtrar = desde.HasValue && hasta.HasValue;
        var reporte = filtrar
            ? await _socioRepositorio.ObtenerReporteAsync(desde, hasta)
            : await _socioRepositorio.ObtenerReporteAsync(null, null);

        return View(reporte);
    }
}
