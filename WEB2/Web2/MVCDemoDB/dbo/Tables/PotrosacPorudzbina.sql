CREATE TABLE [dbo].[PotrosacPorudzbina]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [KorisnickoIme] NVARCHAR(50) NOT NULL, 
    [IdPorudzbine] INT NOT NULL, 
    CONSTRAINT [FK_KorPor_Potrosaci] FOREIGN KEY ([KorisnickoIme]) REFERENCES [Potrosaci]([KorisnickoIme]), 
    CONSTRAINT [FK_KorPor_Porudzbine] FOREIGN KEY ([IdPorudzbine]) REFERENCES [Porudzbine]([IdPorudzbine]), 
    
)
