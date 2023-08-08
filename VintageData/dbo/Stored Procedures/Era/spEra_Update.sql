CREATE PROCEDURE [dbo].[spEra_Update]
	@Id INT,
	@Name NVARCHAR(50),
	@Description NVARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Era]
	SET [Name] = @Name,
		[Description] = @Description
	WHERE Id = @Id;

	SELECT @Id AS 'InsertedId';

	RETURN 0;
END