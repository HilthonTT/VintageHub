CREATE PROCEDURE [dbo].[spOrder_Insert]
	@UserId INT,
	@TotalPrice MONEY,
	@IsComplete BIT,
	@IsCanceled BIT,
	@DateOrdered DATETIME2
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @InsertedId INT;

	INSERT INTO [dbo].[Order] ([UserId], [TotalPrice], [IsComplete], [IsCanceled], [DateOrdered])
	VALUES (@UserId, @TotalPrice, @IsComplete, @IsCanceled, @DateOrdered)

	SET @InsertedId = SCOPE_IDENTITY();

	SELECT @InsertedId AS 'InsertedId';

	RETURN 0;
END
