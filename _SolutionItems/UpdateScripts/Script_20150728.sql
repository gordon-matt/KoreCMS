BEGIN TRANSACTION UpgradeDB
GO

ALTER TABLE [dbo].[Kore_Common_Regions]
ADD [Order] smallint NULL
GO

INSERT INTO [dbo].[Kore_Version]([Version])
VALUES('2015-07-28')
GO

COMMIT TRANSACTION UpgradeDB;
GO