CREATE PROCEDURE [Auth].[CreateOrganizationUser]
	@userId INT,
	@organizationId INT,
	@roleId INT,
	@employeeId NVARCHAR(128) = NULL
AS
BEGIN

	INSERT INTO [Auth].[OrganizationUser]
		([UserId],
		[OrganizationId],
		[OrganizationRoleId],
		[EmployeeId],
		[MaxAmount]
		)
	VALUES
		(@userId,
		@organizationId,
		@roleId,
		@employeeId,
		0);
	Declare @subs Table (SubscriptionId INT)
	/*Unassigned for all subscription in organizaion*/

	Insert Into @subs(SubscriptionId)
	SELECT SubscriptionID from
		[Billing].[Subscription]
	where 
		Subscription.OrganizationId = @organizationId
	;

	Insert Into [Billing].[SubscriptionUser]([ProductRoleId],[SubscriptionId],[UserId]) 
	SELECT 0, SubscriptionId,@userId
	FROM @subs;

END
