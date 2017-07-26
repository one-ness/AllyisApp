CREATE TABLE [Lookup].[State] (
    [StateId] INT            IDENTITY (1, 1) NOT NULL,
    [Code]    NVARCHAR (4)   NOT NULL,
    [Name]    NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_State] PRIMARY KEY NONCLUSTERED ([StateId] ASC),
    CONSTRAINT [FK_States_Country] FOREIGN KEY ([Code]) REFERENCES [Lookup].[Country] ([Code])
);




GO
CREATE CLUSTERED INDEX [IX_State_Code]
    ON [Lookup].[State]([Code] ASC);

