CREATE PROCEDURE [dbo].[spReview_GetByArtifactIdDetailed]
	@ArtifactId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		R.[Id], R.[Title], R.[Description], R.[Rating],
		U.[Id] AS [Id], U.[ObjectIdentifier] AS [ObjectIdentifier], U.[FirstName] AS [FirstName], 
		U.[LastName] AS [LastName], U.[DisplayName] AS [DisplayName], 
		U.[EmailAddress] AS [EmailAddress], U.[Address] AS [Address],
		A.[Id] AS [Id], A.[Name] AS [Name], A.[Description] AS [Description], 
		A.[ImageId] AS [ImageId], A.[Quantity] AS [Quantity], A.[Rating] AS [Rating],
        A.[Price] AS [Price], A.[DiscountAmount] AS [DiscountAmount], A.[Availability] AS [Availability]
	FROM [dbo].[Review] R
	INNER JOIN [dbo].[User] U ON R.[UserId] = U.Id
	INNER JOIN [dbo].[Artifact] A ON R.[ArtifactId] = A.Id
	WHERE [R].[ArtifactId] = @ArtifactId;

	RETURN 0;
END