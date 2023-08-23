CREATE PROCEDURE [dbo].[spVendor_Insert]
	@OwnerUserId INT,
	@Name NVARCHAR(100),
	@Description NVARCHAR(1000),
	@ImageId NVARCHAR(MAX),
	@DateFounded DATETIME2
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @InsertedId INT;

	INSERT INTO [dbo].[Vendor] ([OwnerUserId], [Name], [Description], [ImageId], [DateFounded])
	VALUES (@OwnerUserId, @Name, @Description, @ImageId, @DateFounded);

	SET @InsertedId = SCOPE_IDENTITY();

	SELECT @InsertedId AS 'InsertedId';

	RETURN 0;
END