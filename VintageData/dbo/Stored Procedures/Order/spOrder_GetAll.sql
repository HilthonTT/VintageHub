CREATE PROCEDURE [dbo].[spOrder_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [UserId], [TotalPrice], [IsComplete], [IsCanceled], [DateOrdered]
	FROM [dbo].[Order]

	RETURN 0;
END