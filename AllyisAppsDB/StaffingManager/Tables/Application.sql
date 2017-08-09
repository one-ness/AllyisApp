CREATE TABLE [StaffingManager].[Application] (
	[ApplicationId]          INT            IDENTITY (111874, 7) NOT NULL,
	[ApplicantId]            INT            NOT NULL,
	[PositionId]             INT            NOT NULL,
	[ApplicationCreatedUtc]  DATETIME2 (0)  NOT NULL DEFAULT (getutcdate()),
	[ApplicationModifiedUtc] DATETIME2 (0)  NOT NULL DEFAULT (getutcdate()),
	[ApplicationStatusId]    TINYINT        NOT NULL DEFAULT ((1)),
	[Notes]                  NVARCHAR (MAX) NULL,
	CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED ([ApplicationId] ASC),
	CONSTRAINT [FK_Application_Applicant] FOREIGN KEY ([ApplicantId]) REFERENCES [StaffingManager].[Applicant] ([ApplicantId]),
	CONSTRAINT [FK_Application_Position] FOREIGN KEY ([PositionId]) REFERENCES [StaffingManager].[Position] ([PositionId])
);
