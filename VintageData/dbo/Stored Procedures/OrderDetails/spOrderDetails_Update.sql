CREATE PROCEDURE [dbo].[spOrderDetails_Update]
	@Id INT,
	@Quantity INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[OrderDetails]
	SET [Quantity] = @Quantity
	WHERE Id = @Id;

	SELECT @Id AS 'SelectedId'

	RETURN 0;
END
