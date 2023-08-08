CREATE PROCEDURE [dbo].[spCategory_Update]
	@Id INT,
	@Name NVARCHAR(50),
	@Description NVARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Category]
	SET [Name] = @Name,
		[Description] = @Description
	WHERE Id = @Id;

	RETURN 0;
END