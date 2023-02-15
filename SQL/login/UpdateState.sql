declare @@state int;

set @@state = {0}

if (@@state = 1)begin
	UPDATE [dbo].Users set IsLogin = {0}, LastLogin = GETDATE()	where [UserCode] = {1}
end
else begin
	UPDATE [dbo].Users set IsLogin = {0} where [UserCode] = {1}
end