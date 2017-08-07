CREATE PROCEDURE [TimeTracker].[UpdateLockDate]
	@organizationId INT,
	@isLockDateUsed BIT,
	@lockDatePeriod INT,
	@lockDateQuantity INT
AS
	SET NOCOUNT ON;
	UPDATE [TimeTracker].[Setting]
		SET [IsLockDateUsed] = @isLockDateUsed,
			[LockDatePeriod] = @lockDatePeriod,
			[LockDateQuantity] = @lockDateQuantity
		WHERE [OrganizationId] = @organizationId;