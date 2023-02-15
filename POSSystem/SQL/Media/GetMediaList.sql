select ROW_NUMBER() OVER (ORDER BY id desc ) AS SrNo,
MediaName_enUS,
isnull(MediaUrl_enUS,'\MediaFolder\sample\images.png') as MediaUrl_enUS,
ID 
from [dbo].MediaInfo  