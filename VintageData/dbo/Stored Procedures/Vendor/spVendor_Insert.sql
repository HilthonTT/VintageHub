CREATE PROCEDURE [dbo].[spVendor_Insert]
	@OwnerUserId INT,
	@Name NVARCHAR(50),
	@Description NVARCHAR(256),
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