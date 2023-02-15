declare @@passOri varchar(20);

if (@@passOri = '')begin
	--SELECT 
	--	C.Code as DefaultCompany ,U.* 
	--FROM Users U inner join CompanyUser C 
	--on U.[UserCode] = C.[UserCode]
	-- WHERE U.DeletedStatus = {0} and  U.[UserCode] = {1} and U.Password = 0

	 select 'vs' as DefaultCompany,* from [dbo].Users
	 WHERE isnull(DeletedStatus,0) = {0} and  [UserCode] = {1} and [Password] = 0
end
else begin
	--SELECT 
	--	C.Code as DefaultCompany ,U.* 
	--FROM Users U inner join CompanyUser C 
	--on U.[UserCode] = C.[UserCode]
	-- WHERE U.DeletedStatus = {0} and  U.[UserCode] = {1} and U.Password = {2}
	 SELECT 
		'vs' as DefaultCompany,* 
	FROM [dbo].Users 
	 WHERE isnull(DeletedStatus,0) = {0} and [UserCode] = {1} and [Password] = {2}
end

