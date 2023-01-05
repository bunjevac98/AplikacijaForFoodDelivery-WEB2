CREATE TABLE [dbo].[Administratori]
(
    [KorisnickoIme] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [Lozinka] NVARCHAR(50) NULL, 
    [Ime] NVARCHAR(50) NULL, 
    [Prezime] NVARCHAR(50) NULL, 
    [Adresa] NVARCHAR(50) NULL, 
    [Email] NVARCHAR(50) NULL, 
    [DatumRodjenja] DATETIME NULL, 
    [TipKorisnika] NVARCHAR(50) NULL, 
    [Slika] NVARCHAR(50) NULL, 
    [Status] NVARCHAR(50) NULL 
)
