CREATE TABLE [dbo].[OrderHeader]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [OrderId] NVARCHAR(50) NOT NULL UNIQUE, 
    [UserId] NVARCHAR(50) NOT NULL, 
    [StateCode] INT NOT NULL DEFAULT 0, 
    [Remark] NVARCHAR(50) NULL, 
    [UpdateTime] DATETIME NOT NULL DEFAULT GETDATE(), 
    [CreateTime] DATETIME NOT NULL DEFAULT GETDATE(),  
    [FailCode] INT NULL DEFAULT 0, 
    [OrderTypeCode] INT NOT NULL,
    [IsPaid] BIT NULL DEFAULT 0, 
    [IsSent] BIT NULL DEFAULT 0, 
    [IsArrived] BIT NULL DEFAULT 0, 
    [IsCancel] BIT NULL DEFAULT 0, 
    [IsReturn] BIT NULL DEFAULT 0, 
    [IsFinish] BIT NULL DEFAULT 0, 
    [IsDelete] BIT NULL DEFAULT 0,
)
