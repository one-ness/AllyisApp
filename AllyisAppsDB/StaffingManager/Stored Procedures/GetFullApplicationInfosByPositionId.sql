﻿CREATE PROCEDURE [StaffingManager].[GetFullApplicationInfosByPositionId]
	@positionId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Application].[ApplicationId],
		[Application].[ApplicantId],
		[Application].[ApplicationStatusId],
		[Application].[ApplicationModifiedUtc],
		[Application].[Notes]
	FROM [StaffingManager].[Application] WHERE [PositionId] = @positionId
	
	SELECT 
		[Applicant].[ApplicantId],
		[Applicant].[FirstName],
		[Applicant].[LastName],
		[Address].[City],
		[Address].[CountryCode],
		[Address].[StateId],
		[Applicant].[Email],
		[Applicant].[PhoneNumber],
		[Applicant].[Notes]
	FROM [StaffingManager].[Application] 
		Join [StaffingManager].[Applicant] on [Application].[ApplicantId] = [Applicant].[ApplicantId]
		Join [Lookup].[Address] on [Applicant].[AddressId] = [Address].[AddressId]
		WHERE [Application].[PositionId] = @positionId
		
	SELECT 
		[ApplicationDocument].[ApplicationId],
		[ApplicationDocument].[ApplicationDocumentId],
		[ApplicationDocument].[DocumentLink],
		[ApplicationDocument].[DocumentName]
	FROM [StaffingManager].[ApplicationDocument] 
		Join [StaffingManager].[Application] on [Application].[ApplicationId] = [ApplicationDocument].[ApplicationId]
		WHERE [Application].[PositionId] = @positionId
END
