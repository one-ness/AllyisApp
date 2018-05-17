CREATE PROCEDURE [Auth].[SetupOrganization]
	@userId INT,
	@roleId INT,
	@organizationName NVARCHAR(100),
	@siteUrl NVARCHAR(100),
	@address NVARCHAR(100),
	@city NVARCHAR(100),
	@stateId INT,
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
	declare @organizationId int
	set @organizationId = null

	BEGIN TRANSACTION
		-- create organization
		exec @organizationId = [Auth].[CreateOrganization] @organizationName, @siteUrl, @address, @city, @stateId, @countryCode, @postalCode, @phoneNumber, @faxNumber, @subdomainName;

		-- init default information for org: payclasses, employee type
		EXEC [Hrm].[CreateDefaultPayClasses] @organizationId;
		declare @employeeTypeId int
		EXEC @employeeTypeId = [Hrm].[CreateEmployeeType] @organizationId, 'Full Time Employee';

		-- add the payclasses to the employee type
		EXEC [Hrm].[AddOrgPayClassesToEmployeeType] @employeeTypeId, @organizationId

		-- finally, add the user to the org
		EXEC [Auth].[CreateOrganizationUser] @userId, @organizationId, @roleId, @employeeTypeId, @employeeId;

	COMMIT TRANSACTION

	-- return the new organization id
	return @organizationId
END
