CREATE PROCEDURE [dbo].[spWishlist_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[Wishlist]
	WHERE [Id] = @Id;

	SELECT @Id AS 'InsertedId';

	RETURN 0;
END