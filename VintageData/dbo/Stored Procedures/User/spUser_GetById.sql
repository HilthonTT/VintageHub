CREATE PROCEDURE [dbo].[spUser_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [ObjectIdentifier], [FirstName], [LastName], [DisplayName], [EmailAddress], [Address]
	FROM [dbo].[User]
	WHERE Id = @Id;

	RETURN 0;
END