CREATE PROCEDURE [dbo].[spCategory_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [Name], [Description] 
	FROM [dbo].[Category];

	RETURN 0;
END