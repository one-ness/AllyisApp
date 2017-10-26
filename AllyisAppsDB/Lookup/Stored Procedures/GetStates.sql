CREATE PROCEDURE [Lookup].[GetStates]
	@countryCode varchar(8)
AS
BEGIN
	SET NOCOUNT ON;
	select * from [State] with (nolock) where [State].CountryCode = @countryCode
	order by CountryCode, StateName asc
END