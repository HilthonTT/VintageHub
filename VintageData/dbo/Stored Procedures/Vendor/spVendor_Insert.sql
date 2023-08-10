CREATE PROCEDURE [dbo].[spVendor_Insert]
	@OwnerUserId INT,
	@Name NVARCHAR(50),
	@Description NVARCHAR(256),
	@DateFounded DATETIME2
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @InsertedId INT;

	INSERT INTO [dbo].[Vendor] ([OwnerUserId], [Name], [Description], [DateFounded])
	VALUES (@OwnerUserId, @Name, @Description, @DateFounded);

	SET @InsertedId = SCOPE_IDENTITY();

	SELECT @InsertedId AS 'InsertedId';

	RETURN 0;
END