CREATE PROCEDURE [dbo].[spArtifact_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT [Id], [Name], [Description], [ImageUrl], [Quantity], [Price], [CategoryId], [EraId], [Availability]
	FROM [dbo].[Artifact]
	WHERE Id = @Id;

	RETURN 0;
END