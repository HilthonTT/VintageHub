CREATE PROCEDURE [dbo].[spArtifact_Update]
	@Id INT,
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

	UPDATE [dbo].[Artifact]
	SET [Name] = @Name,
		[Description] = @Description,
		[ImageId] = @ImageId,
		[Quantity] = @Quantity,
		[Rating] = @Rating,
		[Price] = @Price,
		[DiscountAmount] = @DiscountAmount,
		[VendorId] = @VendorId,
		[CategoryId] = @CategoryId,
		[EraId] = @EraId,
		[Availability] = @Availability
	WHERE Id = @Id;

	SELECT @Id AS 'InsertedId';

	RETURN 0;
END
