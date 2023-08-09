CREATE PROCEDURE [dbo].[spReview_Insert]
	@UserId INT,
	@ArtifactId INT,
	@Title NVARCHAR(50),
	@Description NVARCHAR(256),
	@IsPositive BIT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @InsertedId INT;

	INSERT INTO [dbo].[Review] ([UserId], [ArtifactId], [Title], [Description], [IsPositive])
	VALUES (@UserId, @ArtifactId, @Title, @Description, @IsPositive)

	SET @InsertedId = SCOPE_IDENTITY();

	SELECT @InsertedId AS 'InsertedId';

	RETURN 0;
END