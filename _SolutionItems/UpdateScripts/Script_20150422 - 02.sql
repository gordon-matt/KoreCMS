EXEC sp_rename 'dbo.Kore_Widgets', 'Kore_ContentBlocks';
GO

EXEC sp_rename 'dbo.Kore_ContentBlocks.WidgetName', 'BlockName', 'COLUMN';
GO

EXEC sp_rename 'dbo.Kore_ContentBlocks.WidgetType', 'BlockType', 'COLUMN';
GO

EXEC sp_rename 'dbo.Kore_ContentBlocks.WidgetValues', 'BlockValues', 'COLUMN';
GO

UPDATE dbo.Kore_ContentBlocks
SET BlockValues = REPLACE(BlockValues, '[[WidgetZone', '[[ContentZone')
GO

UPDATE [dbo].[Kore_ContentBlocks]
SET BlockName = 'Html Block', BlockType = 'Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.HtmlBlock, Kore.Web.ContentManagement'
WHERE BlockName = 'Html Widget'
GO

UPDATE [dbo].[Kore_ContentBlocks]
SET BlockName = 'Form Block', BlockType = 'Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.FormBlock, Kore.Web.ContentManagement'
WHERE BlockName = 'Form Widget'
GO

UPDATE [dbo].[Kore_ContentBlocks]
SET BlockName = 'Lucene Search', BlockType = 'Kore.Indexing.Lucene.LuceneSearchBlock, Kore.Indexing.Lucene'
WHERE BlockName = 'Search Form Widget'
GO
