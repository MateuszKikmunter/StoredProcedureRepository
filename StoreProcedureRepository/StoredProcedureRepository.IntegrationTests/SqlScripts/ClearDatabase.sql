﻿-- Drop CreateEmployees Stored Procedure
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.CreateEmployees') AND type in (N'P', N'PC'))
	DROP PROCEDURE CreateEmployees;

-- Drop Employees Table
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Employees' AND TABLE_SCHEMA = 'dbo')
    DROP TABLE dbo.Employees;

-- Drop Employee User Defined Table Type 
IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name ='EmployeeTableType')
	DROP TYPE [dbo].[EmployeeTableType];