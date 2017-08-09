CREATE TABLE [StaffingManager].[Applicant] (
	[ApplicantId] INT            NOT NULL IDENTITY (111873, 7),
	[AddressId]   INT            NULL,
	[FirstName]   NVARCHAR (32)  NOT NULL,
	[LastName]    NVARCHAR (32)  NOT NULL,
	[Email]       NVARCHAR (100) NOT NULL,
	[PhoneNumber] VARCHAR (16)   NULL,
	[Notes]       NVARCHAR (MAX) NULL,
	CONSTRAINT [PK_Applicant] PRIMARY KEY CLUSTERED ([ApplicantId] ASC),
	CONSTRAINT [FK_Applicant_Address] FOREIGN KEY ([AddressId]) REFERENCES [Lookup].[Address] ([AddressId]),
);
