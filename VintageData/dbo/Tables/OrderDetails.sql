CREATE TABLE [dbo].[OrderDetails]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OrderId] INT NULL, 
    [ArtifactId] INT NULL, 
    [Quantity] INT NOT NULL, 
    CONSTRAINT [FK_OrderDetails_ToOrder] FOREIGN KEY ([OrderId]) REFERENCES [Order]([Id]) ON DELETE SET NULL, 
    CONSTRAINT [FK_OrderDetails_ToArtifact] FOREIGN KEY ([ArtifactId]) REFERENCES [Artifact]([Id]) ON DELETE SET NULL,
)
