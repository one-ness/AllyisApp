create procedure Lookup.GetAllStates
as
begin
	set nocount on
	select * from State with (nolock)
end