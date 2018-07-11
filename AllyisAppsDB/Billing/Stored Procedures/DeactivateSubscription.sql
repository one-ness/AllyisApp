CREATE PROCEDURE [Billing].[DeactivateSubscription]
	@subscriptionId INT,
	@orgId INT
AS
BEGIN
	SET NOCOUNT ON;

	--BEGIN
	Begin Transaction
	
	--DELETE SUBSCRIPTION-USER ENTRIES RELATED TO SUBSCRIPTION ID
	DELETE FROM SubscriptionUser WHERE [SubscriptionId] = @subscriptionId
	
	--DELETE STAFFING-TRACKER ENTRIES RELATED TO SUBSCRIPTION ID
	declare @status INT = (select [DefaultApplicationStatusId] FROM [Staffing].[StaffingSettings] WHERE [SubscriptionId] = @subscriptionId)
	DELETE FROM [Staffing].[StaffingSettings] WHERE [SubscriptionId] = @subscriptionId
	DELETE FROM [Staffing].[ApplicationStatus] WHERE [ApplicationStatusId] = @status

	--DELETE TIME-TRACKER ENTRIES RELATED TO SUBSCRIPTION ID
	DELETE FROM [TimeTracker].[Setting] WHERE @orgId = [OrganizationId]

	--DELETE THE SUBSCRIPTION ROW FROM SUBSCRIPTION
	DELETE FROM Subscription WHERE [SubscriptionId] = @subscriptionId

	--COMMIT
	Commit Transaction
END