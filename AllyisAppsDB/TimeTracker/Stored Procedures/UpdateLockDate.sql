CREATE PROCEDURE [TimeTracker].[UpdateLockDate]
	@OrganizationId INT,
	@IsLockDateUsed BIT,
	@LockDatePeriod INT,
	@LockDateQuantity INT
AS
	SET NOCOUNT ON;
	UPDATE [TimeTracker].[Setting]
		SET [IsLockDateUsed] = @IsLockDateUsed,
			[LockDatePeriod] = @LockDatePeriod,
			[LockDateQuantity] = @LockDateQuantity
		WHERE [OrganizationId] = @OrganizationId;