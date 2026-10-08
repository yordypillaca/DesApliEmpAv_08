using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Web.Controllers;

public class LibrosController : Controller
{
    private readonly LibroRepositorio _libroRepositorio;

    public LibrosController(LibroRepositorio libroRepositorio)
    {
        _libroRepositorio = libroRepositorio;
    }

    public async Task<IActionResult> Index(string? titulo)
    {
        ViewData["Title"] = "Libros";
        ViewData["TituloBusqueda"] = titulo ?? string.Empty;

        if (string.IsNullOrWhiteSpace(titulo))
        {
            var listado = await _libroRepositorio.ListarAsync();
            return View(listado);
        }

        var resultados = await _libroRepositorio.BuscarPorTituloAsync(titulo.Trim());
        return View("Buscar", resultados);
    }

    public async Task<IActionResult> Details(int id)
    {
        ViewData["Title"] = "Detalle del libro";

        var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
        if (libro == null)
        {
            return NotFound();
        }

        return View(libro);
    }

    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Nuevo libro";
        await CargarAutoresAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Libro libro)
    {
        ViewData["Title"] = "Nuevo libro";

        if (!ModelState.IsValid)
        {
            await CargarAutoresAsync(libro.AutorId);
            return View(libro);
        }

        await _libroRepositorio.InsertarAsync(libro);

        TempData["Mensaje"] = "El libro se registró correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Editar libro";

        var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
        if (libro == null)
        {
            return NotFound();
        }

        await CargarAutoresAsync(libro.AutorId);
        return View(libro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Libro libro)
    {
        ViewData["Title"] = "Editar libro";

        if (id != libro.LibroId)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await CargarAutoresAsync(libro.AutorId);
            return View(libro);
        }

        await _libroRepositorio.ActualizarAsync(libro);

        TempData["Mensaje"] = "El libro se actualizó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Eliminar libro";

        var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
        if (libro == null)
        {
            return NotFound();
        }

        return View(libro);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _libroRepositorio.EliminarAsync(id);

        TempData["Mensaje"] = "El libro se eliminó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarAutoresAsync(int? autorSeleccionado = null)
    {
        var autores = await _libroRepositorio.ListarAutoresAsync();
        ViewData["Autores"] = new SelectList(autores, "AutorId", "Nombre", autorSeleccionado);
    }
}
