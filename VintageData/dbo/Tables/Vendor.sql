CREATE TABLE [dbo].[Vendor]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OwnerUserId] INT NULL,
    [Name] NVARCHAR(100) NOT NULL, 
    [Description] NVARCHAR(1000) NOT NULL, 
    [ImageId] NVARCHAR(MAX) NOT NULL, 
    [DateFounded] DATETIME2 NOT NULL, 
    CONSTRAINT [FK_Vendor_ToUser] FOREIGN KEY ([OwnerUserId]) REFERENCES [User]([Id]) ON DELETE SET NULL, 
)
