CREATE TABLE [dbo].[Review]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [ArtifactId] INT NOT NULL, 
    [Title] NVARCHAR(100) NOT NULL, 
    [Description] NVARCHAR(1000) NOT NULL, 
    [Rating] DECIMAL(18,4) NOT NULL , 
    CONSTRAINT [FK_Review_ToUser] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]), 
    CONSTRAINT [FK_Review_ToArtifact] FOREIGN KEY ([ArtifactId]) REFERENCES [Artifact]([Id]) ON DELETE CASCADE
)
