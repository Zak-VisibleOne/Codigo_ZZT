select UserCode,
UserName,
UserGroupCode,
Email,
'' as PhoneNo,
isnull([ImageUrl],'/images/users/user-4.jpg') as ImageUrl
from  [dbo].Users
where UserCode = {0} 
