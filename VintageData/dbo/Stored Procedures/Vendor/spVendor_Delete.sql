CREATE PROCEDURE [dbo].[spVendor_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[Vendor]
	WHERE Id = @Id;

	SELECT @Id AS 'SelectedId';

	RETURN 0;
END