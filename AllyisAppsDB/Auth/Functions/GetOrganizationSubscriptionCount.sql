create FUNCTION [Auth].[GetOrganizationSubscriptionCount]
(
	@organizationId INT
)
RETURNS SMALLINT
AS
BEGIN
	DECLARE @count SMALLINT

	SELECT @count = COUNT(*)
	FROM [Billing].[Subscription]
	WHERE [OrganizationId] = @organizationId

	RETURN @count
END