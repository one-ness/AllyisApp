﻿CREATE PROCEDURE [Lookup].[GetStateList]
AS
BEGIN
	SET NOCOUNT ON
	SELECT [StateId], [Name], [Code] FROM [State] WITH (NOLOCK)
END
