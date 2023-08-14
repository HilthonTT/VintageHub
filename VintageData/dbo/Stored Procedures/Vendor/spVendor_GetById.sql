CREATE PROCEDURE [dbo].[spVendor_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [OwnerUserId], [Name], [Description], [ImageId], [DateFounded]
	FROM [dbo].[Vendor]
	WHERE Id = @Id;

	RETURN 0;
END
