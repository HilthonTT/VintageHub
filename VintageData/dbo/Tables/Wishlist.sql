CREATE TABLE [dbo].[Wishlist]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [UserId] INT NOT NULL,
    [ArtifactId] INT NOT NULL,
    CONSTRAINT [FK_Wishlist_ToUser] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]),
    CONSTRAINT [FK_Wishlist_ToArtifact] FOREIGN KEY ([ArtifactId]) REFERENCES [Artifact]([Id])
)
