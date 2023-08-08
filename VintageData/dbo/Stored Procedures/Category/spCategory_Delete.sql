CREATE PROCEDURE [dbo].[spCategory_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
		
	DELETE FROM [dbo].[Category]
	WHERE Id = @Id;

	RETURN 0;
END