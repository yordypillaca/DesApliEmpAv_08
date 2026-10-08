using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios;

public class LibroRepositorio
{
    private readonly string _cadenaConexion;

    public LibroRepositorio(IConfiguration configuration)
    {
        _cadenaConexion = configuration.GetConnectionString("BibliotecaDB")
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión BibliotecaDB.");
    }

    public async Task<IEnumerable<Libro>> ListarAsync()
    {
        using var conexion = new SqlConnection(_cadenaConexion);

        return await conexion.QueryAsync<Libro>(
            "usp_Libros_Listar",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Libro>> BuscarPorTituloAsync(string titulo)
    {
        using var conexion = new SqlConnection(_cadenaConexion);

        return await conexion.QueryAsync<Libro>(
            "usp_Libros_Buscar",
            new { Titulo = titulo },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Libro?> ObtenerPorIdAsync(int libroId)
    {
        using var conexion = new SqlConnection(_cadenaConexion);

        return await conexion.QueryFirstOrDefaultAsync<Libro>(
            "usp_Libros_ObtenerPorId",
            new { LibroId = libroId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task InsertarAsync(Libro libro)
    {
        using var conexion = new SqlConnection(_cadenaConexion);

        await conexion.ExecuteAsync(
            "usp_Libros_Insertar",
            new { libro.Titulo, libro.ISBN, libro.AutorId, libro.Ejemplares },
            commandType: CommandType.StoredProcedure);
    }

    public async Task ActualizarAsync(Libro libro)
    {
        using var conexion = new SqlConnection(_cadenaConexion);

        await conexion.ExecuteAsync(
            "usp_Libros_Actualizar",
            new { libro.LibroId, libro.Titulo, libro.ISBN, libro.AutorId, libro.Ejemplares },
            commandType: CommandType.StoredProcedure);
    }

    public async Task EliminarAsync(int libroId)
    {
        using var conexion = new SqlConnection(_cadenaConexion);

        await conexion.ExecuteAsync(
            "usp_Libros_Eliminar",
            new { LibroId = libroId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Autor>> ListarAutoresAsync()
    {
        using var conexion = new SqlConnection(_cadenaConexion);

        return await conexion.QueryAsync<Autor>(
            "usp_Autores_Listar",
            commandType: CommandType.StoredProcedure);
    }
}
