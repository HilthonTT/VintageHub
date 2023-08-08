CREATE PROCEDURE [dbo].[spArtifact_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[Artifact]
	WHERE Id = @Id;

	SELECT @Id AS 'InsertedId';

	RETURN 0;
END