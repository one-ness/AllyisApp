CREATE PROCEDURE [TimeTracker].[GetLockDate]
	@organizationId INT
AS
	SET NOCOUNT ON;
	SELECT [IsLockDateUsed], [LockDatePeriod], [LockDateQuantity] FROM [TimeTracker].[Setting] WITH (NOLOCK) WHERE [OrganizationId] = @organizationId;
