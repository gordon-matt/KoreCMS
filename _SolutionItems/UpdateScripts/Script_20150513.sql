ALTER TABLE Kore_Blog
ADD UseExternalLink bit NOT NULL DEFAULT(0)
GO
ALTER TABLE Kore_Blog
ADD ExternalLink nvarchar(255) NULL
GO