CREATE TABLE [dbo].[Potrosaci]
(
	KorisnickoIme NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [Lozinka] NVARCHAR(MAX) NOT NULL, 
    [Ime] NVARCHAR(50) NOT NULL, 
    [Prezime] NVARCHAR(50) NOT NULL, 
    [Adresa] NVARCHAR(50) NOT NULL, 
    [Email] NVARCHAR(50) NOT NULL, 
    [DatumRodjenja] DATETIME NOT NULL, 
    [TipKorisnika] NVARCHAR(50) NULL, 
    [Slika] NVARCHAR(MAX) NOT NULL, 
    [Status] NVARCHAR(50) NULL, 
    [Porudzbine] NVARCHAR(MAX) NULL
)
