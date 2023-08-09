﻿CREATE PROCEDURE [dbo].[spReview_Update]
	@Id INT,
	@Title NVARCHAR(50),
	@Description NVARCHAR(256),
	@IsPositive BIT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Review]
	SET [Title] = @Title,
		[Description] = @Description,
		[IsPositive] = @IsPositive
	WHERE [Id] = @Id;

	SELECT @Id AS 'InsertedId';

	RETURN 0;
END