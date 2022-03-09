CREATE TABLE [dbo].[LinePayOrder]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
	[OrderId] NVARCHAR(50) NOT NULL UNIQUE, 
    [LinePayOrderId] NVARCHAR(50) NOT NULL, 

	CONSTRAINT [FK_LinePayOrder_OrderHeader] FOREIGN KEY ([OrderId]) REFERENCES [OrderHeader]([OrderId]),
)
