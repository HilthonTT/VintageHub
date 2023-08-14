CREATE PROCEDURE [dbo].[spVendor_GetByOwnerId]
	@OwnerUserId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [OwnerUserId], [Name], [Description], [ImageId], [DateFounded]
	FROM [dbo].[Vendor]
	WHERE OwnerUserId = @OwnerUserId;

	RETURN 0;
END