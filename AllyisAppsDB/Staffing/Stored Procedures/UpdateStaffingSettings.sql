CREATE PROCEDURE [Staffing].[UpdateStaffingSettings]
	@subscriptionId INT,
	@positionStatusId INT
AS
BEGIN
	SET NOCOUNT ON
	UPDATE [Staffing].[StaffingSettings] 
	SET 
		[DefaultPositionStatusId] = @positionStatusId
	WHERE [StaffingSettings].[SubscriptionId] = @subscriptionId
END
