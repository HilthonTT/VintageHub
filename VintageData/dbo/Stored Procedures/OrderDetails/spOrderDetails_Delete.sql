CREATE PROCEDURE [dbo].[spOrderDetails_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[OrderDetails]
	WHERE Id = @Id;

	SELECT @Id AS 'InsertedId';

	RETURN 0;
END