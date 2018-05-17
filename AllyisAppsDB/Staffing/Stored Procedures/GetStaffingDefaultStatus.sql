CREATE PROCEDURE [Staffing].[GetStaffingDefaultStatus]
	@subscriptionId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [DefaultPositionStatusId],
		   [DefaultApplicationStatusId]
	FROM [Staffing].[StaffingSettings]
	WHERE [StaffingSettings].[SubscriptionId] = @subscriptionId

END