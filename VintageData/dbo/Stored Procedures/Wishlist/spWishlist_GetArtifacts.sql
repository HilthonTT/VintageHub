CREATE PROCEDURE [dbo].[spWishlist_GetArtifacts]
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT A.*
	FROM [dbo].[Artifact] A
	INNER JOIN [dbo].[Wishlist] W ON A.Id = W.ArtifactId
	WHERE W.UserId = @UserId;

	RETURN 0;
END