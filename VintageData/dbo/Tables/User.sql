CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ObjectIdentifier] NVARCHAR(36) NOT NULL, 
    [FirstName] NVARCHAR(100) NOT NULL, 
    [LastName] NVARCHAR(100) NOT NULL, 
    [DisplayName] NVARCHAR(100) NOT NULL, 
    [EmailAddress] NVARCHAR(256) NOT NULL, 
    [Address] NVARCHAR(200) NOT NULL
)
