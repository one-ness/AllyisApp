CREATE TABLE [StaffingManager].[Tag] (
	[TagId]				INT				 IDENTITY (13301, 7) NOT NULL,
	[TagName]			NVARCHAR (32)	 NOT NULL,
	CONSTRAINT [PK_TagId] PRIMARY KEY CLUSTERED ([TagId] ASC),
);

