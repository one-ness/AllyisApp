CREATE FUNCTION [Crm].[GetProjectCount]
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

	RETURN @count
END
