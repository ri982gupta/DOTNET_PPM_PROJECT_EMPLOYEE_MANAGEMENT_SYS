-- Create Database
CREATE DATABASE PPM_DB;
GO

-- Use the database
USE PPM_DB;
GO

-- Create Projects Table
CREATE TABLE Projects (
    ProjectId INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    CreatedOn DATETIME DEFAULT GETDATE(),
    ModifiedOn DATETIME DEFAULT GETDATE()
);
GO

-- Create Employees Table
CREATE TABLE Employees (
    EmployeeId INT PRIMARY KEY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Email NVARCHAR(100),
    Mobile NVARCHAR(20),
    Address NVARCHAR(255),
    RoleId INT,
    CreatedOn DATETIME DEFAULT GETDATE(),
    ModifiedOn DATETIME DEFAULT GETDATE()
);
GO

-- Create Roles Table
CREATE TABLE Roles (
    RoleId INT,
    Name NVARCHAR(50) NOT NULL,
    CreatedOn DATETIME DEFAULT GETDATE(),
    ModifiedOn DATETIME DEFAULT GETDATE()
);
GO

-- Create the ProjectEmployees table to manage the many-to-many relationship
CREATE TABLE ProjectEmployees (
    ProjectId INT,
    EmployeeId INT,
	
    PRIMARY KEY (ProjectId, EmployeeId),
    FOREIGN KEY (ProjectId) REFERENCES Projects(ProjectId),
    FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId)
);

select * from ProjectEmployees


-- Create Foreign Key Constraints

-- Projects Table
ALTER TABLE Projects
ADD CONSTRAINT FK_Projects_Roles FOREIGN KEY (RoleId) REFERENCES Roles(RoleId);
GO

-- Employees Table
ALTER TABLE Employees
ADD CONSTRAINT FK_Employees_Roles FOREIGN KEY (RoleId) REFERENCES Roles(RoleId);
GO

-- Create stored procedure

-- Insert project

CREATE PROCEDURE InsertProject
    @ProjectId INT,
    @Name NVARCHAR(100),
    @StartDate DATE,
    @EndDate DATE
    
AS
BEGIN
    INSERT INTO Projects (ProjectId, Name, StartDate, EndDate)
    VALUES (@ProjectId, @Name, @StartDate, @EndDate);
END;



-- Update project

CREATE PROCEDURE UpdateProjects
    @ProjectId INT,
    @Name NVARCHAR(100),
    @StartDate DATE,
    @EndDate DATE
  
AS
BEGIN
    UPDATE Projects
    SET ProjectId = @ProjectId, Name = @Name, StartDate = @StartDate, EndDate = @EndDate
	WHERE ProjectId = @ProjectId;
END;



-- Delete project

CREATE PROCEDURE DeleteProject
    @Id INT
AS
BEGIN
    DELETE FROM Projects WHERE ProjectId = @Id;
END;

-- List all project

CREATE PROCEDURE SelectAllProjects
AS
BEGIN
    SELECT * FROM Projects;
END;

-- List project by Id

CREATE PROCEDURE SelectProjectById
    @Id INT
AS
BEGIN
    SELECT * FROM Projects WHERE ProjectId = @Id;
END;

-- View projects

CREATE PROCEDURE ViewProjects
AS
BEGIN
    SELECT * FROM Projects;
END;

-- Insert employee

CREATE PROCEDURE InsertEmployee
      @EmployeeId INT ,
	  @FirstName NVARCHAR(100),
	  @LastName NVARCHAR(100),
	  @Email NVARCHAR(100),
	  @Mobile BIGINT,
	  @Address NVARCHAR(100),
	  @RoleId INT
AS
BEGIN
      INSERT INTO Employees(EmployeeId, FirstName, LastName, Email, Mobile, Address, RoleId)
	  VALUES (@EmployeeId, @FirstName, @Lastname, @Email, @Mobile, @Address, @RoleId);
END;

-- Update employee

CREATE PROCEDURE UpdateEmployee
      @Id INT,
	  @FirstName NVARCHAR(100),
	  @LastName NVARCHAR(100),
	  @Email NVARCHAR(100),
	  @Mobile BIGINT,
	  @Address NVARCHAR(100),
	  @RoleId INT
AS
BEGIN
      UPDATE Employees 
	  SET EmployeeId = @Id, FirstName = @FirstName, LastName = @LastName, Email = @Email, Mobile = @Mobile, @Address = Address, @RoleId =RoleId
	  WHERE EmployeeId = @Id;
END;



-- Delete employee

CREATE PROCEDURE DeleteEmployee
    @Id INT
AS
BEGIN
    DELETE FROM Employees WHERE EmployeeId = @Id;
END;

-- List all project

CREATE PROCEDURE SelectAllEmployees
AS
BEGIN
    SELECT * FROM Employees;
END;

-- List project by Id

CREATE PROCEDURE SelectEmployeeById
    @Id INT
AS
BEGIN
    SELECT * FROM Employees WHERE EmployeeId = @Id;
END;

-- View employees

CREATE PROCEDURE ViewEmployees
AS
BEGIN
    SELECT * FROM Employees;
END;

-- Insert role

CREATE PROCEDURE InsertRoles
      @RoleId INT,
	  @Name VARCHAR(255)
AS
BEGIN
      INSERT INTO Roles(RoleId, Name)
	  VALUES (@RoleId, @Name);
END;

-- Update role

CREATE PROCEDURE UpdateRoles
      @RoleId INT,
	  @Name NVARCHAR(100)
AS
BEGIN
      UPDATE roles 
	  SET RoleId = @RoleId, Name = @Name
	  WHERE RoleId = @RoleId;
END;

-- Delete role

CREATE PROCEDURE DeleteRole
    @Id INT
AS
BEGIN
    DELETE FROM Roles WHERE RoleId = @Id;
END;

-- List all role

CREATE PROCEDURE SelectAllRoles
AS
BEGIN
    SELECT * FROM Roles;
END;

-- List role by Id

ALTER PROCEDURE SelectRoleById
    @Id INT
AS
BEGIN
    SELECT * FROM Roles WHERE RoleId = @Id;
END;

-- View roles

CREATE PROCEDURE ViewRoles
AS
BEGIN
    SELECT * FROM Roles;
END;

-- Create EmployeesType table

CREATE TYPE dbo.EmployeesType AS TABLE
(
    EmployeeId INT
);


Alter PROCEDURE AssignEmployeeToProject
    @ProjectId INT,
    @EmployeeId INT,
    @Employees EmployeesType READONLY
AS
BEGIN
    -- Insert the assignments into the ProjectEmployees table
    INSERT INTO ProjectEmployees (ProjectId, EmployeeId)
    VALUES (@ProjectId, @EmployeeId);

    -- Insert additional employees from the list
    INSERT INTO ProjectEmployees (ProjectId, EmployeeId)
    SELECT @ProjectId, EmployeeId
    FROM @Employees;
END


-- Stored procedure to remove an employee from a project

CREATE PROCEDURE RemoveEmployeeFromProject
    @ProjectId INT,
    @EmployeeId INT
AS
BEGIN
    DELETE FROM ProjectEmployees
    WHERE ProjectId = @ProjectId AND EmployeeId = @EmployeeId;
END;

-- Stored procedure to view project details

CREATE PROCEDURE ViewProjectDetails
    @ProjectId INT
AS
BEGIN
    -- Declare variables to store project details
	DECLARE @ProjectName VARCHAR(255)
	DECLARE @StartDate DATETIME
	DECLARE @EndDate DATETIME

	-- Select project Details
	SELECT @ProjectName = Name, @StartDate = StartDate, @EndDate = EndDate
	FROM Projects
	WHERE ProjectId = @ProjectId;

	--Check if the project exists
	IF (@ProjectName IS NOT NULL)
	BEGIN
	    
		-- Display project details
		PRINT 'Project Details:';
		PRINT 'Project Name: ' + @ProjectName;
		PRINT 'Start Date: ' + CONVERT(VARCHAR, @StartDate, 120);
		PRINT 'End Date: ' + CONVERT(VARCHAR, @EndDate, 120);

		-- Display assigned employees
		PRINT 'Assigned Employees in the Project:';

		SELECT E.EmployeeId, E.FirstName, E.LastName
		FROM Employees AS E
		JOIN ProjectEmployees AS PE ON E.EmployeeId = PE.EmployeeId
		WHERE PE.ProjectId = @ProjectId;
	END
	ELSE
	BEGIN
	    -- Project not found
		PRINT 'Project not found.';
	END
END;

-- Stored procedure to view project employee details

CREATE PROCEDURE ViewProjectEmployeeDetails
    @ProjectId INT
AS
BEGIN
    SELECT
        P.ProjectId,
        P.Name,
        P.StartDate,
        P.EndDate,
        E.EmployeeId,
        E.FirstName,
        E.LastName,
        E.Email,
        E.Mobile,
        E.Address
    FROM
        Projects P
    LEFT JOIN
        ProjectEmployees PE ON P.ProjectId = PE.ProjectId
    LEFT JOIN
        Employees E ON PE.EmployeeId = E.EmployeeId
    WHERE
        P.ProjectId = @ProjectId;
END;

-- Retrive projects data

SELECT * FROM Projects;

-- Retrive employees data

SELECT * FROM Employees;

-- Retrive roles data

SELECT * FROM Roles;

-- Retrive projects with employee data

SELECT * FROM ProjectEmployees;

--Delete project table
DROP TABLE Projects;

--Delete employee table
DROP TABLE Employees;

--Delete role table
DROP TABLE Roles;

--Delete projectemployee table
DROP TABLE ProjectEmployees;















    


