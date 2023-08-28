CREATE PROCEDURE [dbo].[spReview_GetByIdDetailed]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		R.[Id], R.[Title], R.[Description], R.[Rating],
		U.Id AS [Id], U.[ObjectIdentifier], U.[FirstName], U.[LastName], U.[DisplayName], U.[EmailAddress], U.[Address],
		A.[Id] AS [Id], A.[Name], A.[Description], A.[ImageId], A.[Quantity], A.[Rating],
        A.[Price], A.[DiscountAmount], A.[Availability]
	FROM [dbo].[Review] R
	INNER JOIN [dbo].[User] U ON R.[UserId] = U.Id
	INNER JOIN [dbo].[Artifact] A ON R.[ArtifactId] = A.Id
	WHERE [R].[Id] = @Id;

	RETURN 0;
END