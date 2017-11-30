﻿CREATE PROCEDURE [Auth].[SetupOrganization]
	@userId INT,
	@roleId INT,
	@organizationName NVARCHAR(100),
	@siteUrl NVARCHAR(100),
	@address NVARCHAR(100),
	@city NVARCHAR(100),
	@stateID INT,
	@countryCode VARCHAR(8),
	@postalCode NVARCHAR(50),
	@phoneNumber VARCHAR(50),
	@faxNumber VARCHAR(50),
	@subdomainName NVARCHAR(40),
	@employeeId NVARCHAR(16)
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRANSACTION
		-- Create organization
		EXEC [Auth].[CreateOrganization] @organizationName, @siteUrl, @address, @city, @stateID, @countryCode, @postalCode, @phoneNumber, @faxNumber, @subdomainName;

		-- get the new organization id
		DECLARE @organizationId INT = IDENT_CURRENT('[Auth].[Organization]');
		
		-- Init default pay classes for org
		EXEC [Hrm].[CreateDefaultPayClass] @organizationId;


		EXEC [Hrm].[CreateEmployeeType] @organizationId, 'Full Time Employee';

		DECLARE @employeeTypeId INT = IDENT_CURRENT('[Hrm].[EmployeeType]');

		EXEC [Hrm].[AddPayClassToEmployeeType] @employeeTypeId, 1;
		EXEC [Hrm].[AddPayClassToEmployeeType] @employeeTypeId, 2;
		EXEC [Hrm].[AddPayClassToEmployeeType] @employeeTypeId, 3;
		EXEC [Hrm].[AddPayClassToEmployeeType] @employeeTypeId, 4;
		EXEC [Hrm].[AddPayClassToEmployeeType] @employeeTypeId, 5;
		EXEC [Hrm].[AddPayClassToEmployeeType] @employeeTypeId, 6;
		EXEC [Hrm].[AddPayClassToEmployeeType] @employeeTypeId, 7;
		EXEC [Hrm].[AddPayClassToEmployeeType] @employeeTypeId, 8;

		-- Add user to the org
		EXEC [Auth].[CreateOrganizationUser] @userId, @organizationId, @roleId, @employeeTypeId, @employeeId;


	COMMIT TRANSACTION

	-- return the new organization id
	SELECT @organizationId;
END
