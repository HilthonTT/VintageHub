CREATE PROCEDURE [dbo].[spArtifact_GetByVendorId]
	@VendorId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [Name], [Description], [ImageId], [Quantity], [Rating], [Price], 
			[DiscountAmount], [VendorId], [CategoryId], [EraId], [Availability],
			CASE WHEN [DiscountAmount] = 0 THEN [Price] ELSE [Price] - [DiscountAmount] END AS [FinalPrice]
	FROM [dbo].[Artifact]
	WHERE VendorId = @VendorId;

	RETURN 0;
END