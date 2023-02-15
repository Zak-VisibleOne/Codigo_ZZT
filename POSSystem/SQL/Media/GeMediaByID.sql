SELECT 
	   [ID]
      ,[MediaName_enUS]
      ,isnull([MediaUrl_enUS],'/MediaFolder/sample.jpg') as MediaUrl_enUS
	  ,MediaTypeName
	  ,Size
	  ,convert(varchar,CreatedDate,103) as CreatedDate    
	  ,isnull(CreatedUser,'') as CreatedUser  
FROM [MediaInfo] where dbo.MediaInfo.[ID] = {0} 