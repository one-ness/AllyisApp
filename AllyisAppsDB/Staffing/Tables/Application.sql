CREATE TABLE [Staffing].[Application] (
	[ApplicationId]          INT            NOT NULL IDENTITY (111874, 7),
	[ApplicantId]            INT            NOT NULL,
	[PositionId]             INT            NOT NULL,
	[ApplicationCreatedUtc]  DATETIME2 (0)  NOT NULL DEFAULT (getutcdate()),
	[ApplicationModifiedUtc] DATETIME2 (0)  NOT NULL DEFAULT (getutcdate()),
	[ApplicationStatusId]    TINYINT        NOT NULL DEFAULT ((1)),
	[Notes]                  NVARCHAR (MAX) NULL,
	CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED ([ApplicationId] ASC),
	CONSTRAINT [FK_Application_Applicant] FOREIGN KEY ([ApplicantId]) REFERENCES [Staffing].[Applicant] ([ApplicantId]),
	CONSTRAINT [FK_Application_Position] FOREIGN KEY ([PositionId]) REFERENCES [Staffing].[Position] ([PositionId])
);
