﻿CREATE PROCEDURE [Lookup].[CreateTag]
	@tagName NVARCHAR(32)

AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [Lookup].[Tag] 
		([TagName])
	VALUES 	 
		(@tagName)

	SELECT IDENT_CURRENT('[Lookup].[TagId]');
END
