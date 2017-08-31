CREATE TABLE [StaffingManager].[PositionStatus] (
	[PositionStatusId]		INT				 IDENTITY (13222, 3) NOT NULL,
	[OrganizationId]		INT				 CONSTRAINT [DF_PositionStatus_Organization] DEFAULT ((0)) NOT NULL,
	[PositionStatusName]	NVARCHAR (32)	 NOT NULL,

	CONSTRAINT [PK_PositionStatusId] PRIMARY KEY CLUSTERED ([PositionStatusId] ASC),
	CONSTRAINT [FK_PositionStatus_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]),
);

