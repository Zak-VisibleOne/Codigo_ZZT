CREATE TABLE [ace_report].[MediaInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MediaName_enUS] [nvarchar](max) NULL,
	[MediaUrl_enUS] [nvarchar](max) NULL,
	[MediaType] [int] NULL,
	[MediaTypeName] [nvarchar](max) NULL,
	[Size] [nvarchar](max) NULL,
	[Width] [int] NULL,
	[Height] [int] NULL,
	[Type] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[LastEdited] [datetime] NULL,
	[CreatedUser] [nvarchar](50) NULL,
	[UpdatedUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_MediaInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON,
OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


