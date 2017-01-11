-- First: Manually change "RoleNameIndex" index to include TenantId

DECLARE @TenantId int
SET @TenantId =
(
	SELECT TOP 1 [Id]
	FROM [dbo].[Kore_Tenants]
)

INSERT INTO [dbo].[AspNetRoles]([Id], [Name], [TenantId])
VALUES(NEWID(), 'Administrators', @TenantId)

DECLARE @AdminRoleId uniqueidentifier
SET @AdminRoleId =
(
	SELECT TOP 1 [Id]
	FROM [dbo].[AspNetRoles]
	WHERE [Name] = 'Administrators'
	AND [TenantId] = @TenantId
)

INSERT INTO [dbo].[AspNetUserRoles]([UserId], [RoleId])
	SELECT U.[Id], @AdminRoleId
	FROM [dbo].[AspNetUsers] U
	INNER JOIN [dbo].[AspNetUserRoles] UR ON U.[Id] = UR.[UserId]
	INNER JOIN [dbo].[AspNetRoles] R ON UR.[RoleId] = R.[Id]
	WHERE R.[Name] = 'Administrators'
	AND R.[TenantId] IS NULL
