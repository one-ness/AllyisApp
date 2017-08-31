CREATE TABLE [StaffingManager].[ApplicationDocument] (
	[ApplicationDocumentId] INT            NOT NULL IDENTITY (111872, 7),
	[ApplicationId]         INT            NOT NULL,
	[DocumentLink]          NVARCHAR (100) NOT NULL,
	[DocumentName]          NVARCHAR (32)  NOT NULL,
	CONSTRAINT [PK_ApplicationDocument] PRIMARY KEY CLUSTERED ([ApplicationDocumentId] ASC),
	CONSTRAINT [PK_ApplicationDocument_Application] FOREIGN KEY ([ApplicationId]) REFERENCES [StaffingManager].[Application]([ApplicationId]) ON DELETE CASCADE
);
