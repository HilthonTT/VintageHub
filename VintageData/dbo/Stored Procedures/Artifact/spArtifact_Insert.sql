﻿CREATE PROCEDURE [dbo].[spArtifact_Insert]
	@Name NVARCHAR(50),
	@Description NVARCHAR(500),
	@ImageUrl NVARCHAR(MAX),
	@Quantity INT,
	@Price MONEY,
	@CategoryId INT,
	@EraId INT,
	@Availability BIT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @InsertedId INT;

	INSERT INTO [dbo].[Artifact]([Name], [Description], [ImageUrl], [Quantity], [Price], [CategoryId], [EraId], [Availability])
	VALUES (@Name, @Description, @ImageUrl, @Quantity, @Price, @CategoryId, @EraId, @Availability);

	SET @InsertedId = SCOPE_IDENTITY();

	SELECT @InsertedId AS 'InsertedId';

	RETURN 0;
END
