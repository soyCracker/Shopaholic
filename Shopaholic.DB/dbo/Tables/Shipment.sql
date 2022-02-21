CREATE TABLE [dbo].[Shipment]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [ShipNumber] NVARCHAR(50) NULL, 
    [ReceiveMan] NVARCHAR(50) NOT NULL, 
    [Phone] NVARCHAR(50) NOT NULL, 
    [Address] NVARCHAR(50) NOT NULL, 
    [Email] NVARCHAR(50) NULL, 
	
)
