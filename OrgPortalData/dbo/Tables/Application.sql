﻿-- DROP Table dbo.Application
CREATE TABLE [dbo].[Application] (
                   [PackageFamilyName]	       NVARCHAR (200) NOT NULL, 
                   [PackageFile]	           NVARCHAR (200) NOT NULL, 
                   [PackageName]	           NVARCHAR (200) NOT NULL, 
                   [Name]                      NVARCHAR (255) NOT NULL,
                   [Version]                   NVARCHAR  (25) NOT NULL,
                   [DisplayName]               NVARCHAR (255) NOT NULL,
                   [Description]		       NVARCHAR(2000)     NULL, 
	               [CertificateFile]           NVARCHAR (255) NOT NULL,
	               [BackgroundColor]	       NVARCHAR  (25) NOT NULL,
                   [ProcessorArchitecture]     NVARCHAR  (25) NOT NULL,
                   [InstallMode]		       NVARCHAR  (50) NOT NULL, 
                   [CategoryID]			       INT			  NOT NULL,
                   [Publisher]                 NVARCHAR (255) NOT NULL,
                   [PublisherId]               NVARCHAR  (70) NOT NULL,
                   [PublisherDisplayName]      NVARCHAR (255) NOT NULL,
                   [DateAdded]			       DATETIME       NOT NULL DEFAULT getdate(),
                   CONSTRAINT [PK_Application] 
                   PRIMARY KEY CLUSTERED 
                   (
	                   [PackageFamilyName] ASC,
	                   [Version] ASC
                   ), 
                   CONSTRAINT   [FK_Application_Category] 
                   FOREIGN KEY ([CategoryID]) 
                   REFERENCES   [Category]([ID]) 
);
GO

CREATE UNIQUE NONCLUSTERED INDEX IX_Application_PackageFamilyNameVersion ON dbo.Application
(
	PackageFamilyName,
	Version
) 
