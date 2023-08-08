CREATE PROCEDURE [dbo].[spEra_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [Name], [Description]
	FROM [dbo].[Era];

	RETURN 0;
END