﻿CREATE TABLE [dbo].[Order]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [IsComplete] BIT NOT NULL DEFAULT 0, 
    [IsCanceled] BIT NOT NULL DEFAULT 0, 
    [DateOrdered] DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT [FK_Order_ToUser] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
)
