Select 
row_number() over(order by id) as SrNo,
isnull(ImageUrl,'/images/users/user.jpg') as ImageUrl,
UserCode,
UserName,
isnull(UserGroupCode,'') as UserGroupCode,
ID,
isnull(DeletedStatus,0) as DeletedStatus
from [dbo].Users where UserGroupCode = {0}
   