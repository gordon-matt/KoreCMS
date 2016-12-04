BEGIN TRANSACTION UpgradeDB
GO

DECLARE @NumberOfTenants int
SET @NumberOfTenants =
(
	SELECT COUNT(*)
	FROM [Kore_Tenants]
)

IF (@NumberOfTenants = 0)
BEGIN
	INSERT INTO [Kore_Tenants]([Name], [Url], [Hosts])
	VALUES('Default', 'my-domain.com', 'my-domain.com')
END
GO

ALTER TABLE [AspNetRoles]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

ALTER TABLE [AspNetUsers]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

ALTER TABLE [Kore_BlogCategories]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

ALTER TABLE [Kore_BlogPosts]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

ALTER TABLE [Kore_BlogTags]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

ALTER TABLE [Kore_Common_Regions]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

ALTER TABLE [Kore_Languages]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

ALTER TABLE [Kore_LocalizableStrings]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

ALTER TABLE [Kore_Log]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

ALTER TABLE [Kore_Menus]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

ALTER TABLE [Kore_MessageTemplates]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

ALTER TABLE [Kore_Pages]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

ALTER TABLE [Kore_PageVersions]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

ALTER TABLE [Kore_QueuedEmails]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

ALTER TABLE [Kore_Settings]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

ALTER TABLE [Kore_SitemapConfig]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

ALTER TABLE [Kore_UserProfiles]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

ALTER TABLE [Kore_Zones]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

ALTER TABLE [Permissions]
ADD [TenantId] int NULL,
FOREIGN KEY([TenantId]) REFERENCES [Kore_Tenants]([Id])
GO

DECLARE @TenantId int
SET @TenantId =
(
	SELECT TOP 1 [Id]
	FROM [Kore_Tenants]
)

-- TODO: Set tenant ID for roles and users manually

UPDATE [Kore_BlogCategories]
SET [TenantId] = @TenantId

UPDATE [Kore_BlogPosts]
SET [TenantId] = @TenantId

UPDATE [Kore_BlogTags]
SET [TenantId] = @TenantId

UPDATE [Kore_Common_Regions]
SET [TenantId] = @TenantId

UPDATE [Kore_Languages]
SET [TenantId] = @TenantId

UPDATE [Kore_LocalizableStrings]
SET [TenantId] = @TenantId

UPDATE [Kore_Log]
SET [TenantId] = @TenantId

UPDATE [Kore_Menus]
SET [TenantId] = @TenantId

UPDATE [Kore_MessageTemplates]
SET [TenantId] = @TenantId

UPDATE [Kore_Pages]
SET [TenantId] = @TenantId

UPDATE [Kore_PageVersions]
SET [TenantId] = @TenantId

UPDATE [Kore_QueuedEmails]
SET [TenantId] = @TenantId

UPDATE [Kore_Settings]
SET [TenantId] = @TenantId

UPDATE [Kore_SitemapConfig]
SET [TenantId] = @TenantId

UPDATE [Kore_UserProfiles]
SET [TenantId] = @TenantId

UPDATE [Kore_Zones]
SET [TenantId] = @TenantId

UPDATE [Permissions]
SET [TenantId] = @TenantId

GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Kore_Version' and xtype='U')
	BEGIN
		CREATE TABLE [dbo].[Kore_Version]
		(
			[Version] [varchar](25) NOT NULL,
			[LastUpdatedDateUtc] [datetime] NOT NULL
		) ON [PRIMARY]

		ALTER TABLE [dbo].[Kore_Version]
		ADD CONSTRAINT [DF_Kore_Version_LastUpdatedDateUtc] DEFAULT (GETDATE()) FOR [LastUpdatedDateUtc]
	END
GO

INSERT INTO [dbo].[Kore_Version]([Version])
VALUES('2016-11-21')
GO

COMMIT TRANSACTION UpgradeDB;
GO

