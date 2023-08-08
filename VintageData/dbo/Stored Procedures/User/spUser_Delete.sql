CREATE PROCEDURE [dbo].[spUser_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[User]
	WHERE Id = @Id;

	SELECT @Id AS 'InsertedId';

	RETURN 0;
END