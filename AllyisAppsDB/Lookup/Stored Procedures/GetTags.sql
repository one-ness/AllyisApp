CREATE PROCEDURE [Lookup].[GetTags]
AS
BEGIN
	SELECT * FROM [Lookup].[Tag];
END