CREATE PROCEDURE [StaffingManager].[GetStaffingDefaultStatus]
	@subscriptionId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [DefaultPositionStatusId],
		   [DefaultApplicationStatusId]
	FROM [StaffingManager].[StaffingSettings]
	WHERE [StaffingSettings].[SubscriptionId] = @subscriptionId

END