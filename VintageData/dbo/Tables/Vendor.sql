CREATE TABLE [dbo].[Vendor]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OwnerUserId] INT NOT NULL,
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(256) NOT NULL, 
    [DateFounded] DATETIME2 NOT NULL, 
    CONSTRAINT [FK_Vendor_ToUser] FOREIGN KEY ([OwnerUserId]) REFERENCES [User]([Id]), 
)
