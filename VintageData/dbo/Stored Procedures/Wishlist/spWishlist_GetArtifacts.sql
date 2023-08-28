CREATE PROCEDURE [dbo].[spWishlist_GetArtifacts]
	@UserId INT
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
    INNER JOIN Vendor V ON A.VendorId = V.Id
    INNER JOIN Category C ON A.CategoryId = C.Id
    INNER JOIN Era E ON A.EraId = E.Id
	INNER JOIN [dbo].[Wishlist] W ON A.Id = W.ArtifactId
	WHERE W.UserId = @UserId;

	RETURN 0;
END