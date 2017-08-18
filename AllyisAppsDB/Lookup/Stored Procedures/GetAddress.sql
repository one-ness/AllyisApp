CREATE PROCEDURE [Lookup].[GetAddress]
	@addresId int
AS
BEGIN 
	SELECT * FROM [Lookup].[Address] WHERE [Address].AddressId = @addresId
END
