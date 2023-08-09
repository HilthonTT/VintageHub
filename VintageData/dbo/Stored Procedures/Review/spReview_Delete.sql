CREATE PROCEDURE [dbo].[spReview_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[Review]
	WHERE Id = @Id;

	SELECT @Id AS 'InsertedId';

	RETURN 0;
END