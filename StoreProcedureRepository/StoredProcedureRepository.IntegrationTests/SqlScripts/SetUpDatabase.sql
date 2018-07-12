-- Create Employees Table

CREATE TABLE Employees (
    Id uniqueidentifier PRIMARY KEY,
    Name nvarchar(50) NOT NULL
);
	
ALTER TABLE Employees
ADD CONSTRAINT DF_Id DEFAULT newsequentialid() FOR Id

-- Create Employee User Defined Table Type

 CREATE TYPE [dbo].[EmployeeTableType] AS TABLE
(
    [Id] [uniqueidentifier] NULL,
    [Name] [nvarchar](50) NULL
);

-- Create CreateEmployees Stored Procedure

GO
CREATE PROCEDURE dbo.CreateEmployees(@Employees dbo.EmployeeTableType READONLY)
AS
BEGIN
	INSERT INTO dbo.Employees (Name)
	SELECT Name FROM @Employees;
END

-- Create GetEmployeeByName Stored Procedure

GO
CREATE PROCEDURE dbo.GetEmployeeByName(@EmployeeName nvarchar(50))
AS
BEGIN
	SELECT * FROM Employees WHERE Name = @EmployeeName;
END

-- Create UpdateEmployees Stored Procedure

GO
CREATE PROCEDURE dbo.UpdateEmployees(@EmployeeName nvarchar(50))
AS
BEGIN
	SELECT * FROM Employees WHERE Name = @EmployeeName;
END