CREATE PROCEDURE [dbo].[spOrder_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [UserId], [TotalPrice], [IsComplete], [IsCanceled], [DateOrdered]
	FROM [dbo].[Order]
	WHERE Id = @Id;

	RETURN 0;
END