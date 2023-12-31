﻿CREATE PROCEDURE [dbo].[spReview_Insert]
	@UserId INT,
	@ArtifactId INT,
	@Title NVARCHAR(100),
	@Description NVARCHAR(1000),
	@Rating INT
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