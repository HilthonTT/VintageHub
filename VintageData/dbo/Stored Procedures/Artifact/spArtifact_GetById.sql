CREATE PROCEDURE [dbo].[spArtifact_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT [Id], [Name], [Description], [ImageId], [Quantity], [Rating], [Price], 
			[DiscountAmount], [VendorId], [CategoryId], [EraId], [Availability],
			CASE WHEN [DiscountAmount] = 0 THEN [Price] ELSE [Price] - [DiscountAmount] END AS [FinalPrice]
	FROM [dbo].[Artifact]
	WHERE Id = @Id;

	RETURN 0;
END