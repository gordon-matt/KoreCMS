EXEC sp_rename 'dbo.Kore_Plugins_Google_Sitemap', 'Kore_SitemapConfig';
GO

ALTER TABLE Kore_Blog
ADD MetaKeywords nvarchar(255) NULL
GO

ALTER TABLE Kore_Blog
ADD MetaDescription nvarchar(255) NULL
GO
