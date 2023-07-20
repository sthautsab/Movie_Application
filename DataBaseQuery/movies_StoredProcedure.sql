

ALTER PROCEDURE sp_AddMovie 
@Name nvarchar(max), @Genre nvarchar(max), @Director nvarchar(max) = NULL, @Description nvarchar(max) = NULL, @PhotoPath nvarchar(max) = NULL, @AverageRating decimal = 0
AS
BEGIN
	DECLARE @Idd UNIQUEIDENTIFIER
	SET @Idd = NEWID()	
	INSERT INTO Movies (Id, Name, Genre, Director, Description, PhotoPath, AverageRating) VALUES (@Idd, @Name, @Genre, @Director, @Description, @PhotoPath, @AverageRating)
END


CREATE PROCEDURE sp_GetMovieById 
@Id UNIQUEIDENTIFIER
AS
BEGIN
	SELECT * 
	FROM Movies 
	WHERE Id = @Id
END

CREATE PROCEDURE sp_GetMovies
AS
BEGIN
	SELECT * 
	FROM Movies
END

CREATE PROCEDURE sp_UpdateMovie
@Id UNIQUEIDENTIFIER, @Name nvarchar(max), @Genre nvarchar(max), @Director nvarchar(max), @Description nvarchar(max), @PhotoPath nvarchar(max), @AverageRating decimal
AS
BEGIN
	UPDATE Movies 
	SET Id = @Id,
		Name = @Name,
		Genre = @Genre,
		Director = @Director,
		Description = @Description,
		PhotoPath = @PhotoPath,
		AverageRating = @AverageRating
	WHERE Id = @Id
END

CREATE PROCEDURE sp_DeleteMovie
@Id UNIQUEIDENTIFIER
AS
BEGIN
	DELETE FROM Movies WHERE Id = @Id
END

exec sp_UpdateMovie '0995633B-3367-44F0-416A-08DB84D3B424', 'Hari', 'Nepalii', 'ram', NULL, NULL, 0
exec sp_GetMovies
exec sp_GetMovieById '0995633B-3367-44F0-416A-08DB84D3B424'
exec sp_AddMovie RamHari, Nepal, Hari
exec sp_DeleteMovie '0995633B-3367-44F0-416A-08DB84D3B424'