USE BibliotecaDB;
GO

-- =============================================================
-- LIBROS
-- =============================================================

CREATE OR ALTER PROCEDURE usp_Libros_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT l.LibroId, l.Titulo, l.ISBN, l.AutorId,
           a.Nombre AS NombreAutor, l.Ejemplares, l.Activo
    FROM Libros l
    INNER JOIN Autores a ON a.AutorId = l.AutorId
    WHERE l.Activo = 1
    ORDER BY l.Titulo;
END
GO

CREATE OR ALTER PROCEDURE usp_Libros_Buscar
    @Titulo NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT l.LibroId, l.Titulo, l.ISBN, l.AutorId,
           a.Nombre AS NombreAutor, l.Ejemplares, l.Activo
    FROM Libros l
    INNER JOIN Autores a ON a.AutorId = l.AutorId
    WHERE l.Activo = 1
      AND l.Titulo LIKE '%' + @Titulo + '%'
    ORDER BY l.Titulo;
END
GO

CREATE OR ALTER PROCEDURE usp_Libros_ObtenerPorId
    @LibroId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT l.LibroId, l.Titulo, l.ISBN, l.AutorId,
           a.Nombre AS NombreAutor, l.Ejemplares, l.Activo
    FROM Libros l
    INNER JOIN Autores a ON a.AutorId = l.AutorId
    WHERE l.LibroId = @LibroId AND l.Activo = 1;
END
GO

CREATE OR ALTER PROCEDURE usp_Libros_Insertar
    @Titulo     NVARCHAR(200),
    @ISBN       NVARCHAR(20),
    @AutorId    INT,
    @Ejemplares INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Libros (Titulo, ISBN, AutorId, Ejemplares)
    VALUES (@Titulo, @ISBN, @AutorId, @Ejemplares);
END
GO

CREATE OR ALTER PROCEDURE usp_Libros_Actualizar
    @LibroId    INT,
    @Titulo     NVARCHAR(200),
    @ISBN       NVARCHAR(20),
    @AutorId    INT,
    @Ejemplares INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Libros
    SET Titulo = @Titulo,
        ISBN = @ISBN,
        AutorId = @AutorId,
        Ejemplares = @Ejemplares
    WHERE LibroId = @LibroId;
END
GO

CREATE OR ALTER PROCEDURE usp_Libros_Eliminar
    @LibroId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Libros
    SET Activo = 0
    WHERE LibroId = @LibroId;
END
GO

-- =============================================================
-- AUTORES (lista desplegable)
-- =============================================================

CREATE OR ALTER PROCEDURE usp_Autores_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT AutorId, Nombre, Nacionalidad, Activo
    FROM Autores
    WHERE Activo = 1
    ORDER BY Nombre;
END
GO

-- =============================================================
-- SOCIOS
-- =============================================================

CREATE OR ALTER PROCEDURE usp_Socios_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT SocioId, DNI, Nombre, Email, Activo
    FROM Socios
    WHERE Activo = 1
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE usp_Socios_Insertar
    @DNI    NVARCHAR(8),
    @Nombre NVARCHAR(120),
    @Email  NVARCHAR(120)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Socios (DNI, Nombre, Email)
    VALUES (@DNI, @Nombre, @Email);
END
GO

CREATE OR ALTER PROCEDURE usp_Socios_ExisteDni
    @DNI NVARCHAR(8)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(1)
    FROM Socios
    WHERE DNI = @DNI;
END
GO

-- =============================================================
-- REPORTE DE PRÉSTAMOS
-- =============================================================

CREATE OR ALTER PROCEDURE usp_Prestamos_Reporte
    @Desde DATE = NULL,
    @Hasta DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.PrestamoId,
           s.Nombre AS Socio,
           l.Titulo AS Libro,
           p.FechaPrestamo,
           p.FechaLimite,
           p.Estado,
           d.FechaDevolucion
    FROM Prestamos p
    INNER JOIN DetallePrestamo d ON d.PrestamoId = p.PrestamoId
    INNER JOIN Libros l ON l.LibroId = d.LibroId
    INNER JOIN Socios s ON s.SocioId = p.SocioId
    WHERE (@Desde IS NULL OR p.FechaPrestamo >= @Desde)
      AND (@Hasta IS NULL OR p.FechaPrestamo <= @Hasta)
    ORDER BY p.FechaPrestamo, p.PrestamoId, l.Titulo;
END
GO
