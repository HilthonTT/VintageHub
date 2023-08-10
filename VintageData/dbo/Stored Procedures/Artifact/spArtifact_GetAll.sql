CREATE PROCEDURE [dbo].[spArtifact_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [Name], [Description], [ImageUrl], [Quantity], [Rating], [Price], [VendorId], [CategoryId], [EraId], [Availability]
	FROM [dbo].[Artifact];

	RETURN 0;
END