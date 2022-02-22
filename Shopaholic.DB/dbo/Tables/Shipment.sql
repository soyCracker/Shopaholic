CREATE TABLE [dbo].[Shipment]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [OrderId] NVARCHAR(50) NOT NULL, 
    [ShipNumber] NVARCHAR(50) NULL, 
    [ReceiveMan] NVARCHAR(50) NOT NULL, 
    [Phone] NVARCHAR(50) NOT NULL, 
    [Address] NVARCHAR(50) NOT NULL, 
    [Email] NVARCHAR(50) NULL, 
	
    CONSTRAINT [FK_Shipment_OrderHeader] FOREIGN KEY ([OrderId]) REFERENCES [OrderHeader]([OrderId]),
)
