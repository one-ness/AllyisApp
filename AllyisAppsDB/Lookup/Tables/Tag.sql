CREATE TABLE [Lookup].[Tag] (
	[TagId]				INT				 IDENTITY (1331, 7) NOT NULL,
	[TagName]			NVARCHAR (64)	 NOT NULL,
	CONSTRAINT [PK_TagId] PRIMARY KEY CLUSTERED ([TagId] ASC),
);
go

CREATE UNIQUE NONCLUSTERED INDEX [IX_TagName] on [Lookup].[Tag] ([TagName] ASC) 
