CREATE PROCEDURE [TimeTracker].[UpdateLockDate]
	@OrganizationId INT,
	@LockDateUsed BIT,
	@LockDatePeriod VARCHAR(10),
	@LockDateQuantity INT
AS
	SET NOCOUNT ON;
	UPDATE [TimeTracker].[Setting]
		SET [LockDateUsed] = @LockDateUsed,
			[LockDatePeriod] = @LockDatePeriod,
			[LockDateQuantity] = @LockDateQuantity
		WHERE [OrganizationId] = @OrganizationId;