CREATE PROCEDURE [dbo].[spOrder_GetAllDetailed]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		O.[Id], O.[TotalPrice], O.[IsComplete], O.[IsCanceled], O.[DateOrdered],
		U.[Id] AS [Id], U.[ObjectIdentifier] AS [ObjectIdentifier], U.[FirstName] AS [FirstName], 
		U.[LastName] AS [LastName], U.[DisplayName] AS [DisplayName], 
		U.[EmailAddress] AS [EmailAddress], U.[Address] AS [Address]

	FROM [dbo].[Order] O
	INNER JOIN [dbo].[User] U ON O.UserId = U.Id

	RETURN 0;
END
