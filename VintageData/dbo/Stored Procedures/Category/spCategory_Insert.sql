﻿CREATE PROCEDURE [dbo].[spCategory_Insert]
	@Name NVARCHAR(100),
	@Description NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @InsertedId INT;

    INSERT INTO [dbo].[Category] ([Name], [Description])
    VALUES (@Name, @Description);

    SET @InsertedId = SCOPE_IDENTITY();

    SELECT @InsertedId AS 'InsertedId';

    RETURN 0;
END