ALTER TABLE dbo.Kore_PageTypes
ALTER COLUMN LayoutPath nvarchar(255) NULL
GO

INSERT INTO [dbo].[Kore_Version]([Version])
VALUES('2015-07-03 - 2')
GO