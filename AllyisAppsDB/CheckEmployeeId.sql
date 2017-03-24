CREATE PROCEDURE [Auth].[CheckEmployeeId]
	@OrganizationId int,
	@EmployeeId NVARCHAR(16)
AS
			-- Check for existing employee id
		IF EXISTS (
			SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
			WHERE [OrganizationId] = @OrganizationId AND [EmployeeId] = @EmployeeId
		) OR EXISTS (
			SELECT * FROM [Auth].[Invitation] WITH (NOLOCK)
			WHERE [OrganizationId] = @OrganizationId AND [IsActive] = 1 AND [EmployeeId] = @EmployeeId
		)
			SELECT 1 -- Indicates employee id already taken
