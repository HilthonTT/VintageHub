CREATE TABLE [dbo].[Wishlist]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [UserId] INT NULL,
    [ArtifactId] INT NULL,
    CONSTRAINT [FK_Wishlist_ToUser] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Wishlist_ToArtifact] FOREIGN KEY ([ArtifactId]) REFERENCES [Artifact]([Id]) ON DELETE SET NULL
)
