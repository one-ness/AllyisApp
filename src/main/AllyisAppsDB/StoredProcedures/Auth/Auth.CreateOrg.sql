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
    @retId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION
        INSERT INTO [Auth].[Organization] 
                ([Name], 
                [SiteUrl], 
                [Address], 
                [City], 
                [State], 
                [Country], 
                [PostalCode], 
                [PhoneNumber], 
                [FaxNumber], 
                [Subdomain])
        VALUES (@Name,
                @SiteUrl,
                @Address,
                @City, 
                (SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [Name] = @State), 
                (SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [Name] = @Country),
                @PostalCode,
                @PhoneNumber,
                @FaxNumber,
                @Subdomain);

        SET @retId = SCOPE_IDENTITY();

        EXEC [Auth].[CreateOrgUser] @UserId, @retId, @RoleId;
    COMMIT TRANSACTION

    SELECT SCOPE_IDENTITY();
END
