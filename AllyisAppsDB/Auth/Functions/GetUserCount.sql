﻿CREATE FUNCTION [Auth].[GetUserCount]
(
	@organizationId INT
)
RETURNS SMALLINT
AS
BEGIN
	DECLARE @count SMALLINT

	SELECT @count = COUNT(*)
	FROM [Auth].[OrganizationUser]
	WHERE [OrganizationId] = @organizationId

	RETURN @count
END
