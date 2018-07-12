-- Create Employees Table

CREATE TABLE Employees (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name nvarchar(50) NOT NULL
);

-- Create Employee User Defined Table Type

 CREATE TYPE [dbo].[EmployeeTableType] AS TABLE
(
    [Id] [int] NOT NULL,
    [Name] [nvarchar](50) NULL
    PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )
)

-- Create Employees Stored Procedure

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.CreateEmployees') AND type in (N'P', N'PC'))
  EXEC sp_executesql N'CREATE PROCEDURE dbo.CreateEmployees AS BEGIN RETURN 0 END'
GO
ALTER PROCEDURE dbo.CreateEmployees(@EmployeeTableType dbo.EmployeeTableType READONLY)
AS
BEGIN
	INSERT INTO dbo.Employees (Name)
	SELECT Name FROM @EmployeeTableType;
END
GO