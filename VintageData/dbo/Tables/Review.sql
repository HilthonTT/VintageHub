CREATE TABLE [dbo].[Review]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NULL, 
    [ArtifactId] INT NULL, 
    [Title] NVARCHAR(100) NOT NULL, 
    [Description] NVARCHAR(1000) NOT NULL, 
    [Rating] INT NOT NULL , 
    CONSTRAINT [FK_Review_ToUser] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]) ON DELETE SET NULL, 
    CONSTRAINT [FK_Review_ToArtifact] FOREIGN KEY ([ArtifactId]) REFERENCES [Artifact]([Id]) ON DELETE SET NULL
)
