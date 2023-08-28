CREATE PROCEDURE [dbo].[spWishlist_Insert]
	@UserId INT,
	@ArtifactId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @InsertedId INT;

	INSERT INTO [dbo].[Wishlist] ([UserId], [ArtifactId])
	VALUES (@UserId, @ArtifactId);

	SET @InsertedId = SCOPE_IDENTITY();

	SELECT @InsertedId AS 'InsertedId';

	RETURN 0;
END
