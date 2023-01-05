CREATE TABLE [dbo].[PorudzbinaProizvod]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [IdPorudzbine] INT NOT NULL, 
    [IdProizvoda] INT NOT NULL, 
    CONSTRAINT [FK_PorudzbinaProizvod_Porudzbine] FOREIGN KEY ([IdPorudzbine]) REFERENCES [Porudzbine]([IdPorudzbine]),
    CONSTRAINT [FK_PorudzbinaProizvod_Proizvodi] FOREIGN KEY ([IdProizvoda]) REFERENCES [Proizvodi]([IdProizvoda])




)
