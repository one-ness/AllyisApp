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
	WHERE [customerId] = @customerId
	AND GETUTCDATE() <= [EndUtc]

	RETURN @count
END
