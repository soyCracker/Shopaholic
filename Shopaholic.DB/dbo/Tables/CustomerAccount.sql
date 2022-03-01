CREATE TABLE [dbo].[CustomerAccount]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [AccountId] NVARCHAR(50) NOT NULL,
    [DisplayName] NVARCHAR(50) NULL, 
    [Email] NVARCHAR(50) NOT NULL, 
    [EmailVerified] BIT NOT NULL DEFAULT 0, 
    [PhotoURL] NVARCHAR(MAX) NULL, 
    [Type] NVARCHAR(50) NULL
)
