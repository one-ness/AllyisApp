CREATE procedure [Auth].[GetOrganizationsByIds]
	@csvOrgIds nvarchar(max)
as
begin
	set nocount on
	select o.* from Organization o with (nolock)
	inner join dbo.SplitNumberString(@csvOrgIds) t1 on t1.Number = o.OrganizationId
end