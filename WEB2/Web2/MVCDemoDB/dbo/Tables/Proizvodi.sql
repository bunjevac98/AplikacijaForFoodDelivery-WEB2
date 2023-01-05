CREATE TABLE [dbo].[Proizvodi]
(
	[IdProizvoda] INT NOT NULL PRIMARY KEY,
    [Cena] INT NULL, 
    [Sastojci] NVARCHAR(50) NULL, 
    [ImeProizvoda] NVARCHAR(50) NULL, 
)
