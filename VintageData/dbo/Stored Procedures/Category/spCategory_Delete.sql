﻿CREATE PROCEDURE [dbo].[spCategory_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
		
	DELETE FROM [dbo].[Category]
	WHERE Id = @Id;

	SELECT @Id AS 'InsertedId';

	RETURN 0;
END