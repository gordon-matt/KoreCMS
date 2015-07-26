ALTER TABLE [Kore_Languages]
DROP COLUMN [UniqueSeoCode]
GO
ALTER TABLE [Kore_Languages]
DROP COLUMN [FlagImageFileName]
GO
EXEC sp_rename 'dbo.Kore_MediaParts', 'Kore_Images';
GO
EXEC sp_rename 'dbo.Kore_MediaPartTypes', 'Kore_ImageEntityTypes';
GO