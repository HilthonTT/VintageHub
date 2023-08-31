CREATE TABLE [dbo].[Artifact]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Description] NVARCHAR(1000) NOT NULL, 
    [ImageId] NVARCHAR(MAX) NOT NULL, 
    [Quantity] INT NOT NULL, 
    [Rating] DECIMAL(18,4) NOT NULL, 
    [Price] MONEY NOT NULL, 
    [DiscountAmount] MONEY NOT NULL DEFAULT 0, 
    [VendorId] INT NOT NULL, 
    [CategoryId] INT NOT NULL, 
    [EraId] INT NOT NULL, 
    [Availability] BIT NOT NULL, 
    CONSTRAINT [FK_Artifact_ToCategory] FOREIGN KEY ([CategoryId]) REFERENCES [Category]([Id]) ON DELETE SET NULL, 
    CONSTRAINT [FK_Artifact_ToEra] FOREIGN KEY ([EraId]) REFERENCES [Era]([Id]) ON DELETE SET NULL, 
    CONSTRAINT [FK_Artifact_ToVendor] FOREIGN KEY ([VendorId]) REFERENCES [Vendor]([Id]) ON DELETE SET NULL
)
