CREATE PROCEDURE [dbo].[spEra_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[Era]
	WHERE Id = @Id;

	SELECT @Id AS 'InsertedId';

	RETURN 0;
END