CREATE TABLE [dbo].[Porudzbine]
(
	[IdPorudzbine] INT NOT NULL PRIMARY KEY, 
    [Proizvodi] NVARCHAR(MAX) NULL, 
    [Kolicina] NVARCHAR(MAX) NULL, 
    [Adresa] NVARCHAR(50) NOT NULL, 
    [Komentar] NVARCHAR(50) NULL, 
    [CenaDostave] INT NOT NULL, 
    [UkupnaCena] INT NOT NULL, 
    [StatusPorudzbine] NVARCHAR(50) NULL, 
    [DatumIsporuke] DATETIME NOT NULL
)
