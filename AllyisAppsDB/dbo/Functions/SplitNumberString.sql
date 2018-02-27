

CREATE function [dbo].SplitNumberString (@stringToSplit nvarchar(max))
returns
	@returnList table ([Number] int)
as
begin

declare @name nvarchar(255)
declare @pos int

while len(@stringToSplit) > 0
begin
	select @pos  = charindex(',', @stringToSplit)
	if @pos = 0
		begin
			select @pos = len(@stringToSplit)
			select @name = @stringToSplit
		end
	else
		begin
			select @name = substring(@stringToSplit, 1, @pos-1)
		end

	insert into @returnList select convert(int, @name)
	
	select @stringToSplit = substring(@stringToSplit, @pos+1, len(@stringToSplit)-@pos)
end

return

end