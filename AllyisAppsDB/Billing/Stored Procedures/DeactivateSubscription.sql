CREATE PROCEDURE [Billing].[DeactivateSubscription]
	@subscriptionId INT
AS
BEGIN
	SET NOCOUNT ON;
	Begin Transaction
	DELETE FROM SubscriptionUser WHERE [SubscriptionId] = @subscriptionId
	declare @status INT = (select [DefaultApplicationStatusId] FROM [Staffing].[StaffingSettings] WHERE [SubscriptionId] = @subscriptionId)
	DELETE FROM [Staffing].[StaffingSettings] WHERE [SubscriptionId] = @subscriptionId
	DELETE FROM [Staffing].[ApplicationStatus] WHERE [ApplicationStatusId] = @status 
	DELETE FROM Subscription WHERE [SubscriptionId] = @subscriptionId
	Commit Transaction
END