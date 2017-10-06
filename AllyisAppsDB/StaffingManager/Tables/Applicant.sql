CREATE TABLE [StaffingManager].[Applicant] (
	[ApplicantId]		INT            NOT NULL IDENTITY (111873, 7),
	[OrganizationId]	INT			   CONSTRAINT [DF_Applicant_Organization] DEFAULT ((0)) NOT NULL,
	[AddressId]			INT            NULL,
	[FirstName]			NVARCHAR (32)  NOT NULL,
	[LastName]			NVARCHAR (32)  NOT NULL,
	[Email]				NVARCHAR (100) NOT NULL,
	[PhoneNumber]		VARCHAR (16)   NULL,
	[Notes]				NVARCHAR (MAX) NULL,
	CONSTRAINT [PK_Applicant] PRIMARY KEY CLUSTERED ([ApplicantId] ASC),
	CONSTRAINT [FK_Applicant_Address] FOREIGN KEY ([AddressId]) REFERENCES [Lookup].[Address] ([AddressId]) ON DELETE CASCADE
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Email]
	ON [StaffingManager].[Applicant] ([Email] ASC)

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_AddressId]
	ON [StaffingManager].[Applicant] ([AddressId] ASC)
