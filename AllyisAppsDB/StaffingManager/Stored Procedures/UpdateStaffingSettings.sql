CREATE PROCEDURE [StaffingManager].[UpdateStaffingSettings]
	@subscriptionId INT,
	@positionStatusId INT
AS
BEGIN
	SET NOCOUNT ON
	UPDATE [StaffingManager].[StaffingSettings] 
	SET 
		[DefaultPositionStatusId] = @positionStatusId
	WHERE [StaffingSettings].[SubscriptionId] = @subscriptionId
END
