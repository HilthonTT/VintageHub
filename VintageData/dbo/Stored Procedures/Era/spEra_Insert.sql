CREATE PROCEDURE [dbo].[spEra_Insert]
	@Name NVARCHAR(100),
	@Description NVARCHAR(500)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @InsertedId INT;

	INSERT INTO [dbo].[Era] ([Name], [Description])
	VALUES (@Name, @Description);

	SET @InsertedId = SCOPE_IDENTITY();

	SELECT @InsertedId as 'InsertedId';

	RETURN 0;
END