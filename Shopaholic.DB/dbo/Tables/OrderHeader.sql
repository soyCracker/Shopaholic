CREATE TABLE [dbo].[OrderHeader]
(
	[Id] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [UserId] NVARCHAR(MAX) NOT NULL, 
    [StateCode] INT NOT NULL DEFAULT 0, 
    [Remark] NVARCHAR(MAX) NULL, 
    [UpdateTime] DATETIME NOT NULL DEFAULT GETDATE(), 
    [CreateTime] DATETIME NOT NULL DEFAULT GETDATE(), 
    [ShipNumber] NVARCHAR(MAX) NULL, 
    [FailCode] INT NULL DEFAULT 0, 
    [OrderTypeId] INT NOT NULL,

    [IsPaid] BIT NULL DEFAULT 0, 
    [IsSent] BIT NULL DEFAULT 0, 
    [IsArrived] BIT NULL DEFAULT 0, 
    [IsCancel] BIT NULL DEFAULT 0, 
    [IsReturn] BIT NULL DEFAULT 0, 
    [IsFinish] BIT NULL DEFAULT 0, 
    [IsDelete] BIT NULL DEFAULT 0, 
    CONSTRAINT [FK_OrderHead_OrderType] FOREIGN KEY ([OrderTypeId]) REFERENCES [OrderType]([Id])
)
