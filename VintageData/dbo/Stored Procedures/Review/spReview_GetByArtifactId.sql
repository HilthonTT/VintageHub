CREATE PROCEDURE [dbo].[spReview_GetByArtifactId]
	@ArtifactId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [UserId], [ArtifactId], [Title], [Description], [IsPositive]
	FROM [dbo].[Review]
	WHERE ArtifactId = @ArtifactId;

	RETURN 0;
END