CREATE PROCEDURE [dbo].[spVendor_GetByIdDetailed]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		V.[Id], V.[Name], V.[Description], V.[ImageId], V.[DateFounded],
		U.[Id] AS [Id], U.[ObjectIdentifier] AS [ObjectIdentifier], U.[FirstName] AS [FirstName], 
		U.[LastName] AS [LastName], U.[DisplayName] AS [DisplayName], 
		U.[EmailAddress] AS [EmailAddress], U.[Address] AS [Address]
	FROM [dbo].[Vendor] V
	INNER JOIN [dbo].[User] U ON V.[OwnerUserId] = U.Id
	WHERE V.[Id] = @Id;

	RETURN 0;
END