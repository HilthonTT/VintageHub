CREATE PROCEDURE [dbo].[spEra_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT [Id], [Name], [Description] 
	FROM [dbo].[Era]
	WHERE Id = @Id;

	RETURN 0;
END