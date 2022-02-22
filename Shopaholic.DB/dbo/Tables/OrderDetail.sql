CREATE TABLE [dbo].[OrderDetail]
(	
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [OrderId] NVARCHAR(50) NOT NULL, 
    [ProductId] INT NOT NULL, 
    [Quantity] INT NOT NULL, 
    [CurrentPrice] INT NOT NULL, 
    [RealQuantity] INT NOT NULL DEFAULT 0, 
    [IsBroken] BIT NOT NULL DEFAULT 0, 
    [BrokenQuantity] INT NOT NULL DEFAULT 0,

	[CreateTime] DATETIME NOT NULL DEFAULT GETDATE(), 
    [UpdateTime] DATETIME NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [FK_OrderDetail_Product] FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id]),
    CONSTRAINT [FK_OrderDetail_OrderHeader] FOREIGN KEY ([OrderId]) REFERENCES [OrderHeader]([OrderId]),
)
