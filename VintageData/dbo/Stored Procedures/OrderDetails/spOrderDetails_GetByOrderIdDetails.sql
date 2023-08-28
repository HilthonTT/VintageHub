CREATE PROCEDURE [dbo].[spOrderDetails_GetByOrderIdDetails]
	@OrderId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		OD.[Id], OD.[Quantity],
		O.Id AS [Id], O.UserId AS [UserId], O.IsCanceled AS [IsCanceled], O.IsComplete AS [IsComplete], 
		O.TotalPrice AS [TotalPrice], O.DateOrdered AS [DateOrdered],
		A.[Id] AS [Id], A.[Name] AS [Name], A.[Description] AS [Description], A.[ImageId] AS [ImageId], 
		A.[Quantity] AS [Quantity], A.[Rating] AS [Rating],
        A.[Price] AS [PRICE], A.[DiscountAmount], A.[Availability]
	FROM [dbo].[OrderDetails] OD
	INNER JOIN [dbo].[Order] O ON OD.[OrderId] = O.Id
	INNER JOIN [dbo].[Artifact] A ON OD.[ArtifactId] = A.Id
	WHERE OD.[OrderId] = @OrderId;
	
	RETURN 0;
END
