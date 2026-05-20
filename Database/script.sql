/*
-------------------------------------------------------------------------------
Author:       Yeisson Duvan Rodriguez Herrera
Create Date:  2026-05-20
Description:  Script Saas MUlti Tenant.
              
Modified By:  [Nombre] - [Fecha] - [Motivo del cambio]
-------------------------------------------------------------------------------
*/


IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SaasMultiTenant')
BEGIN
    CREATE DATABASE SaasMultiTenant;
END
GO

USE SaasMultiTenant;
GO


CREATE TABLE dbo.roles
(
	id INT IDENTITY(1,1) PRIMARY KEY, 
	name NVARCHAR(50) NOT NULL,
	description NVARCHAR(500) NULL,
	isActive BIT NOT NULL DEFAULT 1,
	createdAt DATETIME NOT NULL DEFAULT GETDATE() 
);
GO

INSERT INTO dbo.roles (name, description)
VALUES (N'Admin', N'Puede crear, editar, eliminar proyectos');

INSERT INTO dbo.roles (name, description)
VALUES (N'Editor', N'Solo puede crear y editar proyectos');

INSERT INTO dbo.roles (name, description)
VALUES (N'Lector', N'Solo puede ver los proyectos');
GO

CREATE TABLE dbo.users
(
	id INT IDENTITY(1,1) PRIMARY KEY,
	firstname NVARCHAR(100) NOT NULL, 
	lastname NVARCHAR(100) NOT NULL, 
	email NVARCHAR(100) NOT NULL,
	password NVARCHAR(255) NOT NULL, 
	address NVARCHAR(100),
	phone NVARCHAR(20),
	isActive BIT NOT NULL DEFAULT 1,
	createdAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO


CREATE TABLE dbo.workspaces
(
	id INT IDENTITY(1,1) PRIMARY KEY,
	name NVARCHAR(100) NOT NULL, 
	description NVARCHAR(500) NULL, 
	createdAt DATETIME NOT NULL DEFAULT GETDATE(),
	updateAt DATETIME NULL
);

CREATE TABLE dbo.userWorkspaces(
	userId INT NOT NULL FOREIGN KEY REFERENCES users(id) ON DELETE CASCADE, 
	workspaceId INT NOT NULL FOREIGN KEY REFERENCES workspaces(id) ON DELETE CASCADE, 
	roleId INT NOT NULL FOREIGN KEY  REFERENCES roles(id) ON DELETE CASCADE,
	joinedAt DATETIME NOT NULL DEFAULT GETDATE(),

	PRIMARY KEY CLUSTERED (userId, workspaceId)
);


CREATE TABLE dbo.projects
(
	id INT IDENTITY(1,1) PRIMARY KEY, 
	workspaceId INT FOREIGN KEY REFERENCES workspaces(id) ON DELETE CASCADE,
	createdById INT FOREIGN KEY REFERENCES users (id) ON DELETE CASCADE,
	name NVARCHAR(100) NOT NULL, 
	description NVARCHAR(500) NULL, 
	status VARCHAR(20) NOT NULL DEFAULT 'Activo', 
	createdAt DATETIME NOT NULL DEFAULT GETDATE(), 
	updatedAt DATETIME NULL
);