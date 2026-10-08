using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios;

public class SocioRepositorio
{
    private readonly string _cadenaConexion;

    public SocioRepositorio(IConfiguration configuration)
    {
        _cadenaConexion = configuration.GetConnectionString("BibliotecaDB")
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión BibliotecaDB.");
    }

    public async Task<IEnumerable<Socio>> ListarAsync()
    {
        using var conexion = new SqlConnection(_cadenaConexion);

        return await conexion.QueryAsync<Socio>(
            "usp_Socios_Listar",
            commandType: CommandType.StoredProcedure);
    }

    public async Task InsertarAsync(Socio socio)
    {
        using var conexion = new SqlConnection(_cadenaConexion);

        await conexion.ExecuteAsync(
            "usp_Socios_Insertar",
            new { socio.DNI, socio.Nombre, socio.Email },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> ExisteDniAsync(string dni)
    {
        using var conexion = new SqlConnection(_cadenaConexion);

        var cantidad = await conexion.ExecuteScalarAsync<int>(
            "usp_Socios_ExisteDni",
            new { DNI = dni },
            commandType: CommandType.StoredProcedure);

        return cantidad > 0;
    }

    public async Task<IEnumerable<PrestamoReporte>> ObtenerReporteAsync(DateTime? desde, DateTime? hasta)
    {
        using var conexion = new SqlConnection(_cadenaConexion);

        return await conexion.QueryAsync<PrestamoReporte>(
            "usp_Prestamos_Reporte",
            new { Desde = desde?.Date, Hasta = hasta?.Date },
            commandType: CommandType.StoredProcedure);
    }
}
