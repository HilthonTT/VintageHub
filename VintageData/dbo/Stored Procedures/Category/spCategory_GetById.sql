CREATE PROCEDURE [dbo].[spCategory_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT [Id], [Name], [Description]
	FROM [dbo].[Category]
	WHERE Id = @Id;

	RETURN 0;
END