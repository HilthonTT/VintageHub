CREATE PROCEDURE [dbo].[spUser_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [ObjectIdentifier], [FirstName], [LastName], [DisplayName], [EmailAddress], [Address]
	FROM [dbo].[User]

	RETURN 0;
END