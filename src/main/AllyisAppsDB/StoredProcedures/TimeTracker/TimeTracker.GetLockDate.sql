CREATE PROCEDURE [TimeTracker].[GetLockDate]
	@OrganizationId INT
AS
	SET NOCOUNT ON;
	SELECT [LockDateUsed], [LockDatePeriod], [LockDateQuantity] FROM [TimeTracker].[Setting] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId;
