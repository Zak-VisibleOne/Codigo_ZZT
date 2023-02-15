Select 
row_number() over(order by id) as SrNo,
GroupCode,
GroupName,
ID
from [dbo].UserGroup
where isnull(DeletedStatus,0)<>1   
