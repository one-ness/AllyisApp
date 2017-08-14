CREATE TABLE [StaffingManager].[Position] (
	[PositionId]					INT		         IDENTITY (123000, 3) NOT NULL,
	[OrganizationId]				INT				 CONSTRAINT [DF_Position_Organization] DEFAULT ((0)) NOT NULL,
	[CustomerId]					INT				 CONSTRAINT [DF_Position_Customer] DEFAULT ((0)) NULL,
	[AddressId]						INT              CONSTRAINT [DF_Position_Address] DEFAULT ((0)) NOT NULL,
	[PositionCreatedUtc]			DATETIME2 (0)    DEFAULT (getutcdate()) NOT NULL,
	[PositionModifiedUtc]			DATETIME2 (0)    DEFAULT (getutcdate()) NOT NULL,
	[StartDate]						DATETIME2 (0)    NULL,
	[PositionStatusId]				TINYINT			 NOT NULL DEFAULT ((1)),
	[PositionTitle]					NVARCHAR (140)   NOT NULL,
	[BillingRateFrequency]			INT				 NULL,
	[BillingRateAmount]				INT				 NULL,
	[DurationMonths]				INT				 NOT NULL,
	[EmploymentType]				TINYINT			 NOT NULL,
	[PositionCount]					INT				 NOT NULL DEFAULT ((1)),
	[RequiredSkills]				NVARCHAR (MAX)   NOT NULL,
	[JobResponsibilities]			NVARCHAR (MAX)   NULL,
	[DesiredSkills]					NVARCHAR (MAX)   NULL,
	[PositionLevelId]				TINYINT   NOT NULL,
	[HiringManager]					NVARCHAR (140)   NULL,
	[TeamName]						NVARCHAR (140)   NULL,
	CONSTRAINT [PK_PositionId] PRIMARY KEY CLUSTERED ([PositionId] ASC),
	CONSTRAINT [FK_Position_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
	CONSTRAINT [FK_Position_Address] FOREIGN KEY ([AddressId]) REFERENCES [Lookup].[Address] ([AddressId]),
	CONSTRAINT [FK_Position_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Crm].[Customer] ([CustomerId]),
);

GO
	CREATE UNIQUE NONCLUSTERED INDEX
	[IX_AddressId]
		ON [StaffingManager].[Position]([AddressId] ASC)

