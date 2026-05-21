/*
-------------------------------------------------------------------------------
Author:       Yeisson Duvan Rodriguez Herrera
Create Date:  2026-05-20
Description:  Script Saas MUlti Tenant.
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

CREATE INDEX idx_users_email ON users(email);
GO

INSERT INTO  dbo.users (firstname, lastname, email, password, phone)
VALUES (N'Yeisson', N'Rodriguez', N'yeissonr@prueba.com', N'PASSWORD', N'calle falsa 123', N'+573000000000');
GO

CREATE TABLE dbo.workspaces
(
	id INT IDENTITY(1,1) PRIMARY KEY,
	name NVARCHAR(100) NOT NULL, 
	description NVARCHAR(500) NULL, 
	createdAt DATETIME NOT NULL DEFAULT GETDATE(),
	updateAt DATETIME NULL
);
GO

INSERT INTO dbo.workspaces (name, description)
VALUES (N'Workspace Alfa', N'Workspace Alfa');

INSERT INTO dbo.workspaces (name, description)
VALUES (N'Workspace Beta', N'Workspace Beta');
GO

CREATE TABLE dbo.userWorkspaces(
	userId INT NOT NULL FOREIGN KEY REFERENCES users(id) ON DELETE CASCADE, 
	workspaceId INT NOT NULL FOREIGN KEY REFERENCES workspaces(id) ON DELETE CASCADE, 
	roleId INT NOT NULL FOREIGN KEY  REFERENCES roles(id) ON DELETE CASCADE,
	joinedAt DATETIME NOT NULL DEFAULT GETDATE(),

	PRIMARY KEY CLUSTERED (userId, workspaceId)
);
GO

INSERT INTO dbo.userWorkspaces (userId, workspaceId , roleId)
VALUES ((SELECT id FROM dbo.users WHERE email = 'yeissonr@prueba.com'), (SELECT id FROM dbo.workspaces WHERE name = 'Workspace Alfa'), (SELECT id FROM dbo.roles WHERE name = 'Admin'));

INSERT INTO dbo.userWorkspaces (userId, workspaceId , roleId)
VALUES ((SELECT id FROM dbo.users WHERE email = 'yeissonr@prueba.com'), (SELECT id FROM dbo.workspaces WHERE name = 'Workspace Beta'), (SELECT id FROM dbo.roles WHERE name = 'Lector'));
GO

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
GO

INSERT INTO dbo.projects(workspaceId, createdById, name, description, status)
VALUES ((SELECT id FROM dbo.workspaces where name = 'Workspace Alfa'), (SELECT id FROM dbo.users WHERE email = 'yeissonr@prueba.com'), N'Rediseño sitio web', N'Renovación completa de la interfaz y experiencia de usuario' , 'Activo');

INSERT INTO dbo.projects(workspaceId, createdById, name, description, status)
VALUES ((SELECT id FROM dbo.workspaces where name = 'Workspace Alfa'), (SELECT id FROM dbo.users WHERE email = 'yeissonr@prueba.com'), N'Migración de base de datos', N'Migrar la base de datos local a la nubbe' , 'Activo');

INSERT INTO dbo.projects(workspaceId, createdById, name, description, status)
VALUES ((SELECT id FROM dbo.workspaces where name = 'Workspace Beta'), (SELECT id FROM dbo.users WHERE email = 'yeissonr@prueba.com'), N'Campaña de marketing', N'Realizar campañas de marketing en redes sociales' , 'Activo');

INSERT INTO dbo.projects(workspaceId, createdById, name, description, status)
VALUES ((SELECT id FROM dbo.workspaces where name = 'Workspace Beta'), (SELECT id FROM dbo.users WHERE email = 'yeissonr@prueba.com'), N'App Movil', N'Desarrollar nuevas funcionalidades para las plataformas moviles' , 'Activo');
GO

