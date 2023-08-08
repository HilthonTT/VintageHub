CREATE PROCEDURE [dbo].[spOrderDetails_Insert]
	@OrderId INT,
	@ArtifactId INT,
	@Quantity INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @InsertedId INT;

	INSERT INTO [dbo].[OrderDetails] ([OrderId], [ArtifactId], [Quantity])
	VALUES (@OrderId, @ArtifactId, @Quantity)

	SET @InsertedId = SCOPE_IDENTITY();

	SELECT @InsertedId AS 'InsertedId';

	RETURN 0;
END