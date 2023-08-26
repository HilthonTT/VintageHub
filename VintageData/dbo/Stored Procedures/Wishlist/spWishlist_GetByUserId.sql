CREATE PROCEDURE [dbo].[spWishlist_GetByUserId]
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [UserId], [ArtifactId]
	FROM [dbo].[Wishlist]
	WHERE [UserId] = @UserId;

	RETURN 0;
END