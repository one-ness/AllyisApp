CREATE PROCEDURE [Auth].[CreateOrganizationUser]
	@userId INT,
	@organizationId INT,
	@roleId INT,
	@employeeTypeId INT,
	@employeeId NVARCHAR(128) = NULL
AS
BEGIN

	INSERT INTO [Auth].[OrganizationUser]
		([UserId],
		[OrganizationId],
		[OrganizationRoleId],
		[EmployeeTypeId],
		[EmployeeId],
		[MaxAmount]
		)
	VALUES
		(@userId,
		@organizationId,
		@roleId,
		@employeeTypeId,
		@employeeId,
		0);
	Declare @subs Table (SubscriptionId INT)
	/*Unassigned for all subscription in organizaion*/

	Insert Into @subs(SubscriptionId)
	SELECT SubscriptionId from
		[Billing].[Subscription]
	where 
		Subscription.OrganizationId = @organizationId
	;

	Insert Into [Billing].[SubscriptionUser]([ProductRoleId],[SubscriptionId],[UserId]) 
	SELECT 0, SubscriptionId,@userId 
	FROM [Billing].[Subscription] 
	where Subscription.OrganizationId = @organizationId

END
