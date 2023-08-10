CREATE PROCEDURE [dbo].[spArtifact_GetByVendorId]
	@VendorId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [Name], [Description], [ImageUrl], [Quantity], [Rating], [Price], [VendorId], [CategoryId], [EraId], [Availability]
	FROM [dbo].[Artifact]
	WHERE VendorId = @VendorId;

	RETURN 0;
END