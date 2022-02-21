CREATE TABLE [dbo].[OrderLog]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [OrderId] NVARCHAR(50) NOT NULL, 
    [StateCode] INT NOT NULL DEFAULT 0, 
    [Remark] NVARCHAR(MAX) NULL,    
    [FailCode] INT NULL DEFAULT 0, 
    [OrderTypeCode] INT NOT NULL,
    [UpdateTime] DATETIME NOT NULL DEFAULT GETDATE(), 
    [CreateTime] DATETIME NOT NULL DEFAULT GETDATE()
	
	CONSTRAINT [FK_OrderLog_OrderHeader] FOREIGN KEY ([OrderId]) REFERENCES [OrderHeader]([Id]),
)
