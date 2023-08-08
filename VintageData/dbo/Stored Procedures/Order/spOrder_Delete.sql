CREATE PROCEDURE [dbo].[spOrder_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[Order]
	WHERE Id = @Id;

	SELECT @Id AS 'InsertedId';

	RETURN 0;
END