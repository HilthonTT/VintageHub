CREATE PROCEDURE [dbo].[spOrder_GetByUserId]
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [UserId], [TotalPrice], [IsComplete], [IsCanceled], [DateOrdered]
	FROM [dbo].[Order]
	WHERE [UserId] = @UserId;

	RETURN 0;
END