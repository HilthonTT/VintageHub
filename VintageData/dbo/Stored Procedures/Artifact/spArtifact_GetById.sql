CREATE PROCEDURE [dbo].[spArtifact_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [Name], [Description], [ImageId], [Quantity], [Rating], [Price], [DiscountAmount], [VendorId], [CategoryId], [EraId], [Availability] 
	FROM [dbo].[Artifact]
	WHERE [Id] = @Id;

	RETURN 0;
END
