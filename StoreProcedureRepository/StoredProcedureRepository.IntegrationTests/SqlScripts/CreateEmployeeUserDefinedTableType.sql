 CREATE TYPE [dbo].[EmployeeTableType] AS TABLE
(
    [Id] [int] NOT NULL,
    [Name] [nvarchar](50) NULL
    PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )
)