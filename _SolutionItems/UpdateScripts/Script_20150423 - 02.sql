CREATE TABLE [dbo].[Kore_Log]
(  
    [Id] uniqueidentifier NOT NULL DEFAULT(NEWID()),
    [EventDateTime] datetime NOT NULL,
    [EventLevel] nvarchar(5) NOT NULL,
    [UserName] nvarchar(255) NOT NULL,
    [MachineName] nvarchar(255) NOT NULL,
    [EventMessage] nvarchar(MAX) NOT NULL,
    [ErrorSource] nvarchar(255) NULL,
    [ErrorClass] nvarchar(512) NULL,
    [ErrorMethod] nvarchar(255) NULL,
    [ErrorMessage] nvarchar(MAX) NULL,
    [InnerErrorMessage] nvarchar(MAX) NULL,
    CONSTRAINT [PK_Kore_Log] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )
)