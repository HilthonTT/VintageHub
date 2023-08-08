CREATE PROCEDURE [dbo].[spArtifact_Update]
	@Id INT,
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

	UPDATE [dbo].[Artifact]
	SET [Name] = @Name,
		[Description] = @Description,
		[ImageUrl] = @ImageUrl,
		[Quantity] = @Quantity,
		[Price] = @Price,
		[CategoryId] = @CategoryId,
		[EraId] = @EraId,
		[Availability] = @Availability
	WHERE Id = @Id;

	SELECT @Id AS 'InsertedId';

	RETURN 0;
END
