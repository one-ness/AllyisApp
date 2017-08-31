CREATE TABLE [StaffingManager].[PositionLevel] (
	[PositionLevelId]		INT				 IDENTITY (13222, 3) NOT NULL,
	[OrganizationId]		INT				 DEFAULT ((0)) NOT NULL,
	[PositionLevelName]		NVARCHAR (32)	 NOT NULL

	CONSTRAINT [PK_PositionLevelId] PRIMARY KEY CLUSTERED ([PositionLevelId] ASC),
	CONSTRAINT [FK_PositionLevel_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId])
);

