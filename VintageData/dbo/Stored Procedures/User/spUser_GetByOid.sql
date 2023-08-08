CREATE PROCEDURE [dbo].[spUser_GetByOid]
	@ObjectIdentifier NVARCHAR(36)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [ObjectIdentifier], [FirstName], [LastName], [DisplayName], [EmailAddress], [Address]
	FROM [dbo].[User]
	WHERE ObjectIdentifier = @ObjectIdentifier;

	RETURN 0;
END