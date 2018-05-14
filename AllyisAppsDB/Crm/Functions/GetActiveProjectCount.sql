CREATE FUNCTION [Crm].[GetActiveProjectCount]
(
	@customerId INT
)
RETURNS SMALLINT
AS
BEGIN
	DECLARE @count SMALLINT

	SELECT @count = COUNT(*)
	FROM [Pjm].[Project]
	WHERE [CustomerId] = @customerId
	AND ([EndUtc] IS NULL OR GETUTCDATE() <= [EndUtc])
	AND ([StartUtc] IS NULL OR GETUTCDATE() >= [StartUtc])

	RETURN @count
END
