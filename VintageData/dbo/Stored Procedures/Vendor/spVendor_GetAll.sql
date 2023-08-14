CREATE PROCEDURE [dbo].[spVendor_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [OwnerUserId], [Name], [Description], [ImageId], [DateFounded]
	FROM [dbo].[Vendor]

	RETURN 0;
END