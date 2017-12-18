BEGIN TRANSACTION UpgradeDB
GO

ALTER TABLE [Kore_Pages]
DROP COLUMN [ShowOnMenus]
GO

ALTER TABLE [Kore_PageVersions]
ADD [ShowOnMenus] bit NOT NULL DEFAULT(1)
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Kore_Version' and xtype='U')
	BEGIN
		CREATE TABLE [dbo].[Kore_Version]
		(
			[Version] [varchar](25) NOT NULL,
			[LastUpdatedDateUtc] [datetime] NOT NULL
		) ON [PRIMARY]

		ALTER TABLE [dbo].[Kore_Version]
		ADD CONSTRAINT [DF_Kore_Version_LastUpdatedDateUtc] DEFAULT (GETDATE()) FOR [LastUpdatedDateUtc]
	END
GO

INSERT INTO [dbo].[Kore_Version]([Version])
VALUES('2017-12-18')
GO

COMMIT TRANSACTION UpgradeDB;
GO