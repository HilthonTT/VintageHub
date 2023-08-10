﻿CREATE TABLE [dbo].[Review]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [ArtifactId] INT NOT NULL, 
    [Title] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(256) NOT NULL, 
    [Rating] DECIMAL NOT NULL, 
    CONSTRAINT [FK_Review_ToUser] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]), 
    CONSTRAINT [FK_Review_ToArtifact] FOREIGN KEY ([ArtifactId]) REFERENCES [Artifact]([Id]) ON DELETE CASCADE
)
