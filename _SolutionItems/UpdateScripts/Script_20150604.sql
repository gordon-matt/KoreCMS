BEGIN TRANSACTION UpgradeDB
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Kore_PageVersions]
(
	[Id] [uniqueidentifier] NOT NULL,
	[PageId] [uniqueidentifier] NOT NULL,
	[CultureCode] [nvarchar](10) NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
	[DateModifiedUtc] [datetime] NOT NULL,
	[Status] [tinyint] NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Slug] [nvarchar](255) NOT NULL,
	[Fields] [nvarchar](max) NULL,
	CONSTRAINT [PK_dbo.Kore_PageVersions] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Kore_PageVersions] WITH CHECK
ADD CONSTRAINT [FK_dbo.Kore_PageVersions_dbo.Kore_Pages_PageId]
FOREIGN KEY([PageId])
REFERENCES [dbo].[Kore_Pages] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Kore_PageVersions]
CHECK CONSTRAINT [FK_dbo.Kore_PageVersions_dbo.Kore_Pages_PageId]
GO

INSERT INTO [dbo].[Kore_PageVersions] ([Id], [PageId], [CultureCode], [DateCreatedUtc], [DateModifiedUtc], [Status], [Title], [Slug], [Fields])
SELECT
	NEWID(),
	[Id],
	[CultureCode],
	[DateCreatedUtc],
	[DateModifiedUtc],
	1,
	[Name],
	[Slug],
	[Fields]
FROM [dbo].[Kore_Pages]
GO

ALTER TABLE [dbo].[Kore_Pages]
DROP COLUMN [Slug]
GO

ALTER TABLE [dbo].[Kore_Pages]
DROP COLUMN [Fields]
GO

ALTER TABLE [dbo].[Kore_Pages]
DROP COLUMN [DateCreatedUtc]
GO

ALTER TABLE [dbo].[Kore_Pages]
DROP COLUMN [DateModifiedUtc]
GO

ALTER TABLE [dbo].[Kore_Pages]
DROP COLUMN [CultureCode]
GO

ALTER TABLE [dbo].[Kore_Pages]
DROP COLUMN [RefId]
GO

DROP TABLE [dbo].[Kore_HistoricPages]
GO

COMMIT TRANSACTION UpgradeDB;
GO