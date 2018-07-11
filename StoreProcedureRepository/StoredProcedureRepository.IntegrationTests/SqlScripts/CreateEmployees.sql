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