--------------------------
declare @FirstName nvarchar(20) = null
declare @LastName nvarchar(20) = null
declare @OffsetRows int  = 0
declare @PageSize int =10
select TeacherId, FirstName, Lastname,
	count(*) over() TotalRowsCount
from Teacher
where FirstName like coalesce(@FirstName, '') + '%'
	and Lastname like coalesce(@Lastname, '') + '%'
order by TeacherId
offset @OffsetRows rows
fetch next @PageSize rows only
---------------------------------------
go
create proc TeacherFilter(
 @FirstName nvarchar(20),
 @LastName nvarchar(20),
 @OffsetRows int  ,
 @PageSize int 
)
as
begin
	select TeacherId, FirstName, Lastname, BirthDate,
		count(*) over() TotalRowsCount
	from Teacher
	where FirstName like coalesce(@FirstName, '') + '%'
		and Lastname like coalesce(@Lastname, '') + '%'
	order by TeacherId
	offset @OffsetRows rows
	fetch next @PageSize rows only
end
------------------
exec TeacherFilter @FirstName= null, @LastName = null, @OffsetRows = 2, @PageSize = 2