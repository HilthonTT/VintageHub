CREATE PROCEDURE [dbo].[spOrder_Update]
	@Id INT,
	@TotalPrice MONEY,
	@IsComplete BIT,
	@IsCanceled BIT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Order]
	SET [TotalPrice] = @TotalPrice,
		[IsComplete] = @IsComplete,
		[IsCanceled] = @IsCanceled
	WHERE Id = @Id;

	SELECT @Id AS 'InsertedId';

	RETURN 0;
END