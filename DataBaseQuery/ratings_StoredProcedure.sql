
CREATE PROCEDURE sp_AddRating 
@UserId UNIQUEIDENTIFIER, @MovieId UNIQUEIDENTIFIER, @Rate int
AS
BEGIN
	INSERT INTO Ratings (UserId, MovieId, Rate) VALUES (@UserId, @MovieId, @Rate)
END

CREATE PROCEDURE sp_GetRatingDataByUserIdAndMovieId 
@UserId nvarchar(max), @MovieId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT *
	FROM Ratings
	WHERE UserId = @UserId AND MovieId = @MovieId
END

ALTER PROCEDURE sp_GetRatingByUserIdAndMovieId 
@UserId nvarchar(max), @MovieId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT Rate
	FROM Ratings
	WHERE UserId = @UserId AND MovieId = @MovieId
END


Alter PROCEDURE sp_GetAverageRating
@MovieId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT CAST(SUM(Rate) AS DECIMAL(10, 2)) / COUNT(Rate) AS AverageRating
	FROM Ratings
	WHERE MovieId = @MovieId
END

CREATE PROCEDURE sp_UpdateRating
@Id int, @UserId UNIQUEIDENTIFIER, @MovieId UNIQUEIDENTIFIER, @Rate int
AS
BEGIN
	UPDATE Ratings 
	SET UserId = @UserId,
		MovieId = @MovieId,
		Rate = @Rate
	WHERE Id = @Id
END



2DAC62C8-E8C7-451E-18CC-08DB8869343A
EXEC sp_GetAverageRating '2DAC62C8-E8C7-451E-18CC-08DB8869343A'