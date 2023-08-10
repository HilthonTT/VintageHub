CREATE PROCEDURE [dbo].[spVendor_Update]
	@Id INT,
	@OwnerUserId INT,
	@Name NVARCHAR(50),
	@Description NVARCHAR(256),
	@DateFounded DATETIME2
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Vendor]
	SET [OwnerUserId] = @OwnerUserId,
		[Name] = @Name,
		[Description] = @Description,
		[DateFounded] = @DateFounded
	WHERE Id = @Id;

	SELECT @Id AS 'InsertedId';

	RETURN 0;
END