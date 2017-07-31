CREATE PROCEDURE [TimeTracker].[UpdateLockDate]
	@OrganizationId INT,
	@LockDateUsed BIT,
	@LockDatePeriod INT,
	@LockDateQuantity INT
AS
	SET NOCOUNT ON;
	UPDATE [TimeTracker].[Setting]
		SET [LockDateUsed] = @LockDateUsed,
			[LockDatePeriod] = @LockDatePeriod,
			[LockDateQuantity] = @LockDateQuantity
		WHERE [OrganizationId] = @OrganizationId;