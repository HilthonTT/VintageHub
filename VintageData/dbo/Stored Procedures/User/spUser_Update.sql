CREATE PROCEDURE [dbo].[spUser_Update]
	@Id INT,
	@FirstName NVARCHAR(100),
	@LastName NVARCHAR(100),
	@DisplayName NVARCHAR(100),
	@EmailAddress NVARCHAR(256),
	@Address NVARCHAR(200)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[User]
	SET [FirstName] = @FirstName,
		[LastName] = @LastName,
		[DisplayName] = @DisplayName,
		[EmailAddress] = @EmailAddress,
		[Address] = @Address
	WHERE Id = @Id;

	SELECT @Id as 'InsertedId';

	RETURN 0;
END