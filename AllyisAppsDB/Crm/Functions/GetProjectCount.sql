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
	WHERE [customerId] = @customerId

	RETURN @count
END
