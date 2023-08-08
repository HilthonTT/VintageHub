CREATE PROCEDURE [dbo].[spUser_Insert]
	@ObjectIdentifier NVARCHAR(36),
	@FirstName NVARCHAR(100),
	@LastName NVARCHAR(100),
	@DisplayName NVARCHAR(100),
	@EmailAddress NVARCHAR(256),
	@Address NVARCHAR(200)
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @InsertedId INT;

	INSERT INTO [dbo].[User] ([ObjectIdentifier], [FirstName], [LastName], [DisplayName], [EmailAddress], [Address])
	VALUES (@ObjectIdentifier, @FirstName, @LastName, @DisplayName, @EmailAddress, @Address);

	SET @InsertedId = SCOPE_IDENTITY();

	SELECT @InsertedId as 'InsertedId';

	RETURN 0;
END