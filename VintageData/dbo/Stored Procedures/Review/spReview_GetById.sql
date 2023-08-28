CREATE PROCEDURE [dbo].[spReview_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [UserId], [ArtifactId], [Title], [Description], [Rating]
	FROM [dbo].[Review]
	WHERE [Id] = @Id;

	RETURN 0;
END