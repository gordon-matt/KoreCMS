BEGIN TRANSACTION UpgradeDB
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

EXEC sp_rename 'dbo.Kore_Blog', 'Kore_BlogPosts';
GO

CREATE TABLE [dbo].[Kore_BlogCategories]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[UrlSlug] [nvarchar](255) NOT NULL,
	CONSTRAINT [PK_Kore_BlogCategories] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Kore_BlogTags]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[UrlSlug] [nvarchar](255) NOT NULL,
	[NumberOfOccurrences] [int] NOT NULL,
	CONSTRAINT [PK_Kore_BlogTags] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Kore_BlogPostTags]
(
	[PostId] [uniqueidentifier] NOT NULL,
	[TagId] [int] NOT NULL,
	CONSTRAINT [PK_Kore_BlogPostTags] PRIMARY KEY CLUSTERED 
	(
		[PostId] ASC,
		[TagId] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Kore_BlogPostTags] WITH CHECK
ADD CONSTRAINT [FK_Kore_BlogPostTags_Kore_BlogPosts] FOREIGN KEY([PostId])
REFERENCES [dbo].[Kore_BlogPosts] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Kore_BlogPostTags] CHECK CONSTRAINT [FK_Kore_BlogPostTags_Kore_BlogPosts]
GO

ALTER TABLE [dbo].[Kore_BlogPostTags] WITH CHECK
ADD CONSTRAINT [FK_Kore_BlogPostTags_Kore_BlogTags] FOREIGN KEY([TagId])
REFERENCES [dbo].[Kore_BlogTags] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Kore_BlogPostTags] CHECK CONSTRAINT [FK_Kore_BlogPostTags_Kore_BlogTags]
GO

INSERT INTO [dbo].[Kore_BlogCategories]([Name], [UrlSlug])
VALUES('Default', 'default')

ALTER TABLE [dbo].[Kore_BlogPosts]
ADD [CategoryId] int NOT NULL DEFAULT(1)
GO

ALTER TABLE [dbo].[Kore_BlogPosts] WITH CHECK
ADD CONSTRAINT [FK_Kore_BlogPosts_Kore_BlogCategories]
FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Kore_BlogCategories] ([Id])
GO

ALTER TABLE [dbo].[Kore_BlogPosts]
CHECK CONSTRAINT [FK_Kore_BlogPosts_Kore_BlogCategories]
GO

INSERT INTO [dbo].[Kore_Version]([Version])
VALUES('2015-07-08')
GO

COMMIT TRANSACTION UpgradeDB;
GO