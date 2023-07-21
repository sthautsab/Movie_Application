
CREATE PROCEDURE sp_AddComment 
@MovieId UNIQUEIDENTIFIER, @UserName nvarchar(max), @Content nvarchar(max), @DatePosted DATETIME
AS
BEGIN
	DECLARE @Idd UNIQUEIDENTIFIER
	SET @Idd = NEWID()	
	INSERT INTO Comments (CommentId, MovieId, UserName, Content, DatePosted) VALUES (@Idd, @MovieId, @UserName, @Content, @DatePosted)
END


ALTER PROCEDURE sp_GetMovieComments
@MovieId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT * 
	FROM Comments
	WHERE MovieId = @MovieId
	ORDER BY DatePosted DESC

END

CREATE PROCEDURE sp_GetCommentById 
@CommentId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT * 
	FROM Comments 
	WHERE CommentId = @CommentId
END

CREATE PROCEDURE sp_UpdateComment
@CommentId UNIQUEIDENTIFIER, @MovieId UNIQUEIDENTIFIER, @UserName nvarchar(max), @Content nvarchar(max), @DatePosted DATETIME
AS
BEGIN
	UPDATE Comments 
	SET CommentId = @CommentId,
		MovieId = @MovieId,
		UserName = @UserName,
		Content = @Content,
		DatePosted = @DatePosted
	WHERE CommentId = @CommentId
END

CREATE PROCEDURE sp_DeleteComment
@CommentId UNIQUEIDENTIFIER
AS
BEGIN
	DELETE FROM Comments WHERE CommentId = @CommentId
END

exec sp_GetMovieComments '0F9616C8-D510-43D0-C146-08DB836E9D6D'