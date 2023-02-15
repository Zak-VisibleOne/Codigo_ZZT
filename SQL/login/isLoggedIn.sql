update [dbo].Users set IsLogin = 0;
SELECT [UserCode] as result, CAST(ISNULL(IsLogin,0) as INT) as cnt FROM [dbo].Users where [UserCode] = {0};