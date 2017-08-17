CREATE TABLE [StaffingManager].[Position] (
	[PositionId]					INT				 IDENTITY (123000, 3) NOT NULL,
	[OrganizationId]				INT				 CONSTRAINT [DF_Position_Organization] DEFAULT ((0)) NOT NULL,
	[CustomerId]					INT				 CONSTRAINT [DF_Position_Customer] DEFAULT ((0)) NULL,
	[AddressId]						INT				 CONSTRAINT [DF_Position_Address] DEFAULT ((0)) NOT NULL,
	[PositionCreatedUtc]			DATETIME2 (0)    DEFAULT (getutcdate()) NOT NULL,
	[PositionModifiedUtc]			DATETIME2 (0)    DEFAULT (getutcdate()) NOT NULL,
	[StartDate]						DATETIME2 (0)    NULL,
	[PositionStatusId]				INT				 CONSTRAINT [DF_Position_PositionStatus] DEFAULT ((0)) NOT NULL,
	[PositionTitle]					NVARCHAR (140)   NOT NULL,
	[BillingRateFrequency]			INT				 NULL,
	[BillingRateAmount]				INT				 NULL,
	[DurationMonths]				INT				 NULL,
	[EmploymentTypeId]				INT				 CONSTRAINT [DF_Position_EmploymentType] DEFAULT ((0)) NOT NULL,
	[PositionCount]					INT				 NOT NULL DEFAULT ((0)),
	[RequiredSkills]				NVARCHAR (MAX)   NOT NULL,
	[JobResponsibilities]			NVARCHAR (MAX)   NULL,
	[DesiredSkills]					NVARCHAR (MAX)   NULL,
	[PositionLevelId]				INT				 CONSTRAINT [DF_Position_PositionLevel] DEFAULT ((0)) NOT NULL,
	[HiringManager]					NVARCHAR (140)   NULL,
	[TeamName]						NVARCHAR (140)   NULL,
	CONSTRAINT [PK_PositionId] PRIMARY KEY CLUSTERED ([PositionId] ASC),
	CONSTRAINT [FK_Position_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
	CONSTRAINT [FK_Position_Address] FOREIGN KEY ([AddressId]) REFERENCES [Lookup].[Address] ([AddressId]),
	CONSTRAINT [FK_Position_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Crm].[Customer] ([CustomerId]),
	CONSTRAINT [FK_Position_PositionStatus] FOREIGN KEY ([PositionStatusId]) REFERENCES [StaffingManager].[PositionStatus] ([PositionStatusId]),
	CONSTRAINT [FK_Position_PositionLevel] FOREIGN KEY ([PositionLevelId]) REFERENCES [StaffingManager].[PositionLevel] ([PositionLevelId]),
	CONSTRAINT [FK_Position_Employment] FOREIGN KEY ([EmploymentTypeId]) REFERENCES [StaffingManager].[EmploymentType] ([EmploymentTypeId])
);

GO
	CREATE UNIQUE NONCLUSTERED INDEX
	[IX_AddressId]
		ON [StaffingManager].[Position]([AddressId] ASC)

