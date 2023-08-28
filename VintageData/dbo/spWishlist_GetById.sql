CREATE PROCEDURE [dbo].[spWishlist_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [UserId], [ArtifactId]
	FROM [dbo].[Wishlist]
	WHERE [Id] = @Id;

	RETURN 0;
END