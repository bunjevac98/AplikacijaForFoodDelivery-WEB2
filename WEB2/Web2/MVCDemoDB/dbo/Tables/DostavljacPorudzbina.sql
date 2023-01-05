CREATE TABLE [dbo].[DostavljacPorudzbina]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [KorisnickoIme] NVARCHAR(50) NULL, 
    [IdPorudzbine] INT NULL, 
    CONSTRAINT [FK_KorPor_Dostavljaci] FOREIGN KEY ([KorisnickoIme]) REFERENCES [Dostavljaci]([KorisnickoIme]),
    CONSTRAINT [FK_KorPor_Porudzbine_Dostavljaca] FOREIGN KEY ([IdPorudzbine]) REFERENCES [Porudzbine]([IdPorudzbine]),
	
)
