CREATE TABLE [dbo].[Kore_Blog]
(
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [nvarchar](255) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[Headline] [nvarchar](128) NOT NULL,
	[Slug] [nvarchar](128) NOT NULL,
	[ShortDescription] [nvarchar](255) NOT NULL,
	[FullDescription] [nvarchar](max) NOT NULL,
	CONSTRAINT [PK_dbo.Kore_Blog] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO