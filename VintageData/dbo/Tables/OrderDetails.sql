CREATE TABLE [dbo].[OrderDetails]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OrderId] INT NOT NULL, 
    [ArtifactId] INT NOT NULL, 
    [Quantity] INT NOT NULL, 
    CONSTRAINT [FK_OrderDetails_ToOrder] FOREIGN KEY ([OrderId]) REFERENCES [Order]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_OrderDetails_ToArtifact] FOREIGN KEY ([ArtifactId]) REFERENCES [Artifact]([Id])
)
