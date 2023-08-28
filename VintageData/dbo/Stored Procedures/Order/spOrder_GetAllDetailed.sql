CREATE PROCEDURE [dbo].[spOrder_GetAllDetailed]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		O.[Id], O.[TotalPrice], O.[IsComplete], O.[IsCanceled], O.[DateOrdered],
		U.Id AS [Id], U.[ObjectIdentifier], U.[FirstName], U.[LastName], U.[DisplayName], U.[EmailAddress], U.[Address]
	FROM [dbo].[Order] O
	INNER JOIN [dbo].[User] U ON O.UserId = U.Id

	RETURN 0;
END
