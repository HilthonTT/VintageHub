CREATE TABLE [dbo].[Artifact]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(256) NOT NULL, 
    [ImageUrl] NVARCHAR(MAX) NOT NULL, 
    [Quantity] INT NOT NULL, 
    [Rating] DECIMAL NOT NULL, 
    [Price] MONEY NOT NULL, 
    [VendorId] INT NOT NULL, 
    [CategoryId] INT NOT NULL, 
    [EraId] INT NOT NULL, 
    [Availability] BIT NOT NULL, 
    CONSTRAINT [FK_Artifact_ToCategory] FOREIGN KEY ([CategoryId]) REFERENCES [Category]([Id]), 
    CONSTRAINT [FK_Artifact_ToEra] FOREIGN KEY ([EraId]) REFERENCES [Era]([Id]), 
    CONSTRAINT [FK_Artifact_ToVendor] FOREIGN KEY ([VendorId]) REFERENCES [Vendor]([Id]) ON DELETE CASCADE
)
