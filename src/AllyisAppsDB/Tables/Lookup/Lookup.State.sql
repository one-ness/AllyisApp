CREATE TABLE [Lookup].[State]
(
	[StateId] INT NOT NULL PRIMARY KEY NONCLUSTERED IDENTITY(1,1),
	[Code] CHAR(2) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [FK_States_Country] FOREIGN KEY ([Code]) REFERENCES [Lookup].[Country]([Code])
)

GO

CREATE CLUSTERED INDEX [IX_State_Code] ON [Lookup].[State] ([Code])
