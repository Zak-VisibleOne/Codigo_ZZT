update [dbo].Users set IsLogin = 0;
SELECT sum(IsLogin) as cnt from [dbo].Users where [UserCode] <> {0}