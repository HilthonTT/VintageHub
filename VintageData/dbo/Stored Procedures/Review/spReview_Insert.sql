CREATE PROCEDURE [dbo].[spReview_Insert]
	@UserId INT,
	@ArtifactId INT,
	@Title NVARCHAR(50),
	@Description NVARCHAR(256),
	@Rating DECIMAL(18,4)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @InsertedId INT;

	INSERT INTO [dbo].[Review] ([UserId], [ArtifactId], [Title], [Description], [Rating])
	VALUES (@UserId, @ArtifactId, @Title, @Description, @Rating)

	SET @InsertedId = SCOPE_IDENTITY();

	SELECT @InsertedId AS 'InsertedId';

	RETURN 0;
END