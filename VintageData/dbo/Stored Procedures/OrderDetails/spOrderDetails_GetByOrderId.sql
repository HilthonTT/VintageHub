CREATE PROCEDURE [dbo].[spOrderDetails_GetByOrderId]
	@OrderId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [OrderId], [ArtifactId], [Quantity] 
	FROM [dbo].[OrderDetails]
	WHERE [OrderId] = @OrderId;

	RETURN 0;
END