BEGIN TRANSACTION UpgradeDB
GO

ALTER TABLE [dbo].[Kore_Common_Regions]
ADD [Order] smallint NULL
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
VALUES('2015-07-28')
GO

COMMIT TRANSACTION UpgradeDB;
GO