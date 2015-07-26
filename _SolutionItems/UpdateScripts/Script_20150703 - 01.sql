SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Kore_Version]
(
	[Version] [varchar](25) NOT NULL,
	[LastUpdatedDateUtc] [datetime] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Kore_Version] ADD CONSTRAINT [DF_Kore_Version_LastUpdatedDateUtc]  DEFAULT (getdate()) FOR [LastUpdatedDateUtc]
GO

INSERT INTO [dbo].[Kore_Version]([Version])
VALUES('2015-07-03')
GO