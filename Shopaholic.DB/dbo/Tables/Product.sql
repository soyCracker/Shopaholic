CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(MAX) NOT NULL, 
    [Description ] NVARCHAR(MAX) NULL, 
    [Content] NVARCHAR(MAX) NULL, 
    [Price] INT NOT NULL, 
    [Stock] INT NOT NULL, 
    [Image] NVARCHAR(MAX) NULL, 
    [CategoryId] INT NULL, 
    [IsDelete] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_Product_Category] FOREIGN KEY ([CategoryId]) REFERENCES [Category]([Id])
)
