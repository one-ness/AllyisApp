CREATE PROCEDURE [Auth].[CreateOrg]
    @UserId INT,
    @RoleId INT,
    @Name NVARCHAR(100),
    @SiteUrl NVARCHAR(100),
    @Address NVARCHAR(100),
    @City NVARCHAR(100),
    @State NVARCHAR(100),
    @Country NVARCHAR(100),
    @PostalCode NVARCHAR(50),
    @PhoneNumber VARCHAR(50),
    @FaxNumber VARCHAR(50),
    @Subdomain NVARCHAR(40),
    @retId INT OUTPUT,
	@EmployeeId NVARCHAR(16),
	@EmployeeTypeId INT,
	@AddressId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

	IF EXISTS (
		SELECT * FROM [Auth].[Organization] WITH (NOLOCK)
		WHERE [Subdomain] = @Subdomain
	)
	BEGIN
		-- Subdomain is not unique
		SET @retId = -1;
	END
	ELSE
	BEGIN
		BEGIN TRANSACTION
			INSERT INTO [Lookup].[Address]
					([Address1],
					[City],
					[State], 
					[CountryId], 
					[PostalCode])
			VALUES(@Address,
					@City, 
					(SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [Name] = @State), 
					(SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [Name] = @Country),
					@PostalCode);	
					
			SET @AddressId = SCOPE_IDENTITY()

			-- Create org
			INSERT INTO [Auth].[Organization] 
					([Name], 
					[SiteUrl], 
					[AddressId],
					[PhoneNumber], 
					[FaxNumber], 
					[Subdomain])
			VALUES (@Name,
					@SiteUrl,
					@AddressId,
					@PhoneNumber,
					@FaxNumber,
					@Subdomain);

			SET @retId = SCOPE_IDENTITY();

			EXEC [Auth].[CreateOrgUser] @UserId, @retId, @RoleId, @EmployeeId, @EmployeeTypeId;

			-- Set users's chosen organization to new org
			UPDATE [Auth].[User]
			SET [ActiveOrganizationId] = @retId
			WHERE [UserId] = @UserId

		COMMIT TRANSACTION

		SELECT @retId;
	END
END