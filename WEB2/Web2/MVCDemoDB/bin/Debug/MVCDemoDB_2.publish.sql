﻿/*
Deployment script for MVCDemoDB

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "MVCDemoDB"
:setvar DefaultFilePrefix "MVCDemoDB"
:setvar DefaultDataPath "C:\Users\basic\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB"
:setvar DefaultLogPath "C:\Users\basic\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Starting rebuilding table [dbo].[Administratori]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Administratori] (
    [KorisnickoIme] NVARCHAR (50) NOT NULL,
    [Lozinka]       NVARCHAR (50) NULL,
    [Ime]           NVARCHAR (50) NULL,
    [Prezime]       NVARCHAR (50) NULL,
    [Adresa]        NVARCHAR (50) NULL,
    [Email]         NVARCHAR (50) NULL,
    [DatumRodjenja] DATETIME      NULL,
    [TipKorisnika]  NVARCHAR (50) NULL,
    [Slika]         NVARCHAR (50) NULL,
    [Status]        NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([KorisnickoIme] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Administratori])
    BEGIN
        INSERT INTO [dbo].[tmp_ms_xx_Administratori] ([KorisnickoIme], [Lozinka], [Ime], [Prezime], [Adresa], [Email], [DatumRodjenja], [TipKorisnika], [Slika], [Status])
        SELECT   [KorisnickoIme],
                 [Lozinka],
                 [Ime],
                 [Prezime],
                 [Adresa],
                 [Email],
                 [DatumRodjenja],
                 [TipKorisnika],
                 [Slika],
                 [Status]
        FROM     [dbo].[Administratori]
        ORDER BY [KorisnickoIme] ASC;
    END

DROP TABLE [dbo].[Administratori];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Administratori]', N'Administratori';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Update complete.';


GO
