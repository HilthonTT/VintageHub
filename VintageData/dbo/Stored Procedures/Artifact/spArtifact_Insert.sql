CREATE PROCEDURE [dbo].[spArtifact_Insert]
	@Name NVARCHAR(100),
	@Description NVARCHAR(1000),
	@ImageId NVARCHAR(MAX),
	@Quantity INT,
	@Rating DECIMAL(18,4),
	@Price MONEY,
	@DiscountAmount MONEY,
	@VendorId INT,
	@CategoryId INT,
	@EraId INT,
	@Availability BIT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @InsertedId INT;

	INSERT INTO [dbo].[Artifact]([Name], [Description], [ImageId], [Quantity], [Rating], [Price], 
			[DiscountAmount], [VendorId], [CategoryId], [EraId], [Availability])
	VALUES (@Name, @Description, @ImageId, @Quantity, @Rating, @Price, 
			@DiscountAmount, @VendorId, @CategoryId, @EraId, @Availability);

	SET @InsertedId = SCOPE_IDENTITY();

	SELECT @InsertedId AS 'InsertedId';

	RETURN 0;
END
