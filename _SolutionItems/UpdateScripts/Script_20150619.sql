SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Kore_EntityTypeContentBlocks](
	[Id] [uniqueidentifier] NOT NULL,
	[EntityType] [varchar](512) NOT NULL,
	[EntityId] [varchar](50) NOT NULL,
	[BlockName] [nvarchar](255) NOT NULL,
	[BlockType] [varchar](1024) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[ZoneId] [uniqueidentifier] NOT NULL,
	[Order] [int] NOT NULL,
	[IsEnabled] [bit] NOT NULL,
	[BlockValues] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Kore_EntityTypeContentBlocks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


