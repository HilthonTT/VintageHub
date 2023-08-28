CREATE PROCEDURE [dbo].[spArtifact_GetByVendorIdDetailed]
	@VendorId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
        A.[Id], A.[Name], A.[Description], A.[ImageId], A.[Quantity], A.[Rating],
        A.[Price], A.[DiscountAmount], A.[Availability],
        V.Id AS [Id], V.[Name] AS [Name], V.[Description] AS [Description],
        C.Id AS [Id], C.[Name] AS [Name], C.[Description] AS [Description],
        E.Id AS [Id], E.[Name] AS [Name], E.[Description] AS [Description]
    FROM Artifact A
    INNER JOIN [dbo].[Vendor] V ON A.VendorId = V.Id
    INNER JOIN [dbo].[Category] C ON A.CategoryId = C.Id
    INNER JOIN [dbo].[Era] E ON A.EraId = E.Id
	WHERE [A].[VendorId] = @VendorId;

	RETURN 0;
END