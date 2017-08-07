﻿CREATE PROCEDURE [Auth].[SetupOrganization]
	@userId INT,
	@roleId INT,
	@organizationName NVARCHAR(100),
	@siteUrl NVARCHAR(100),
	@address NVARCHAR(100),
	@city NVARCHAR(100),
	@state NVARCHAR(100),
	@country NVARCHAR(100),
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
		EXEC [Auth].[CreateOrganization] @organizationName, @siteUrl, @address, @city, @state, @country, @postalCode, @phoneNumber, @faxNumber, @subdomainName;

		-- get the new organization id
		DECLARE @organizationId INT = IDENT_CURRENT('[Auth].[Organization]');

		-- Add user to the org
		EXEC [Auth].[CreateOrganizationUser] @userId, @organizationId, @roleId, @employeeId;

		-- Init default pay classes for org
		EXEC [Hrm].[CreateDefaultPayClass] @organizationId;
	COMMIT TRANSACTION

	-- return the new organization id
	SELECT @organizationId;
END
