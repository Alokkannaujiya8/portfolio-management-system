USE [PortfolioDB]
GO
/****** Object:  Table [dbo].[Admin]    Script Date: 08-03-2026 21:55:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admin](
	[AdminId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[AdminId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlbumItems]    Script Date: 08-03-2026 21:55:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlbumItems](
	[AlbumItemId] [int] IDENTITY(1,1) NOT NULL,
	[AlbumId] [int] NULL,
	[GalleryId] [int] NULL,
	[DisplayOrder] [int] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[AlbumItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogCategories]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogCategories](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
	[Slug] [nvarchar](100) NULL,
	[Description] [nvarchar](500) NULL,
	[Icon] [nvarchar](100) NULL,
	[DisplayOrder] [int] NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogComments]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogComments](
	[CommentId] [int] IDENTITY(1,1) NOT NULL,
	[PostId] [int] NULL,
	[ParentCommentId] [int] NULL,
	[VisitorId] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Website] [nvarchar](500) NULL,
	[Comment] [nvarchar](max) NOT NULL,
	[IsApproved] [bit] NULL,
	[IsSpam] [bit] NULL,
	[IPAddress] [nvarchar](50) NULL,
	[LikeCount] [int] NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CommentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogImages]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogImages](
	[ImageId] [int] IDENTITY(1,1) NOT NULL,
	[PostId] [int] NULL,
	[ImagePath] [nvarchar](500) NOT NULL,
	[ThumbnailPath] [nvarchar](500) NULL,
	[Caption] [nvarchar](500) NULL,
	[AltText] [nvarchar](200) NULL,
	[DisplayOrder] [int] NULL,
	[IsCover] [bit] NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ImageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogLikes]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogLikes](
	[LikeId] [int] IDENTITY(1,1) NOT NULL,
	[PostId] [int] NULL,
	[VisitorId] [int] NULL,
	[IPAddress] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[LikeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogPosts]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogPosts](
	[PostId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NULL,
	[Title] [nvarchar](500) NOT NULL,
	[Slug] [nvarchar](500) NULL,
	[Excerpt] [nvarchar](1000) NULL,
	[Content] [nvarchar](max) NULL,
	[FeaturedImage] [nvarchar](500) NULL,
	[VideoUrl] [nvarchar](500) NULL,
	[VideoEmbedCode] [nvarchar](max) NULL,
	[Tags] [nvarchar](500) NULL,
	[MetaTitle] [nvarchar](200) NULL,
	[MetaDescription] [nvarchar](500) NULL,
	[MetaKeywords] [nvarchar](500) NULL,
	[ViewCount] [int] NULL,
	[LikeCount] [int] NULL,
	[CommentCount] [int] NULL,
	[IsPublished] [bit] NULL,
	[PublishedDate] [datetime] NULL,
	[IsFeatured] [bit] NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[PostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogVideos]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogVideos](
	[VideoId] [int] IDENTITY(1,1) NOT NULL,
	[PostId] [int] NULL,
	[VideoTitle] [nvarchar](500) NULL,
	[VideoUrl] [nvarchar](500) NULL,
	[VideoEmbedCode] [nvarchar](max) NULL,
	[VideoType] [nvarchar](50) NULL,
	[ThumbnailPath] [nvarchar](500) NULL,
	[DisplayOrder] [int] NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[VideoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChatMessages]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatMessages](
	[ChatId] [int] IDENTITY(1,1) NOT NULL,
	[VisitorId] [int] NULL,
	[VisitorName] [nvarchar](100) NULL,
	[VisitorEmail] [nvarchar](100) NULL,
	[Message] [nvarchar](max) NULL,
	[IsFromAdmin] [bit] NULL,
	[IsRead] [bit] NULL,
	[ReadDate] [datetime] NULL,
	[IPAddress] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ChatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactMessages]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactMessages](
	[MessageId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Subject] [nvarchar](200) NULL,
	[Message] [nvarchar](max) NOT NULL,
	[IsRead] [bit] NULL,
	[IsReplied] [bit] NULL,
	[ReplyMessage] [nvarchar](max) NULL,
	[RepliedDate] [datetime] NULL,
	[IPAddress] [nvarchar](50) NULL,
	[UserAgent] [nvarchar](500) NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Education]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Education](
	[EducationId] [int] IDENTITY(1,1) NOT NULL,
	[Degree] [nvarchar](100) NOT NULL,
	[Institute] [nvarchar](200) NOT NULL,
	[Year] [int] NOT NULL,
	[Percentage] [decimal](5, 2) NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[EducationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Experience]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Experience](
	[ExperienceId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [nvarchar](200) NOT NULL,
	[Role] [nvarchar](100) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NULL,
	[Description] [nvarchar](max) NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ExperienceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Gallery]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Gallery](
	[GalleryId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](500) NULL,
	[Description] [nvarchar](max) NULL,
	[MediaType] [nvarchar](50) NULL,
	[MediaPath] [nvarchar](500) NULL,
	[ThumbnailPath] [nvarchar](500) NULL,
	[VideoEmbedCode] [nvarchar](max) NULL,
	[Category] [nvarchar](100) NULL,
	[Tags] [nvarchar](500) NULL,
	[DisplayOrder] [int] NULL,
	[IsFeatured] [bit] NULL,
	[ViewCount] [int] NULL,
	[DownloadCount] [int] NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[GalleryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GalleryAlbums]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GalleryAlbums](
	[AlbumId] [int] IDENTITY(1,1) NOT NULL,
	[AlbumName] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[CoverImage] [nvarchar](500) NULL,
	[Slug] [nvarchar](200) NULL,
	[DisplayOrder] [int] NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[AlbumId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PageVisits]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PageVisits](
	[PageVisitId] [int] IDENTITY(1,1) NOT NULL,
	[VisitorId] [int] NULL,
	[PageUrl] [nvarchar](500) NULL,
	[PageTitle] [nvarchar](200) NULL,
	[TimeSpent] [int] NULL,
	[VisitTime] [datetime] NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[PageVisitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Profile]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profile](
	[ProfileId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Title] [nvarchar](100) NULL,
	[Description] [nvarchar](max) NULL,
	[Email] [nvarchar](100) NULL,
	[Phone] [nvarchar](20) NULL,
	[Address] [nvarchar](200) NULL,
	[LinkedIn] [nvarchar](200) NULL,
	[GitHub] [nvarchar](200) NULL,
	[Photo] [nvarchar](255) NULL,
	[ResumePath] [nvarchar](255) NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProfileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Projects]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projects](
	[ProjectId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectName] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ImagePath] [nvarchar](255) NULL,
	[GitHubLink] [nvarchar](500) NULL,
	[LiveLink] [nvarchar](500) NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResumeViews]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResumeViews](
	[ViewId] [int] IDENTITY(1,1) NOT NULL,
	[VisitorName] [nvarchar](100) NULL,
	[VisitorEmail] [nvarchar](100) NULL,
	[CompanyName] [nvarchar](200) NULL,
	[Designation] [nvarchar](100) NULL,
	[ViewDate] [datetime] NULL,
	[IPAddress] [nvarchar](50) NULL,
	[UserAgent] [nvarchar](500) NULL,
	[Country] [nvarchar](100) NULL,
	[City] [nvarchar](100) NULL,
	[IsDownloaded] [bit] NULL,
	[DownloadDate] [datetime] NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ViewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Skills]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Skills](
	[SkillId] [int] IDENTITY(1,1) NOT NULL,
	[SkillName] [nvarchar](100) NOT NULL,
	[Percentage] [int] NOT NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[SkillId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VisitorTracking]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VisitorTracking](
	[VisitorId] [int] IDENTITY(1,1) NOT NULL,
	[SessionId] [nvarchar](100) NULL,
	[IPAddress] [nvarchar](50) NULL,
	[UserAgent] [nvarchar](500) NULL,
	[Country] [nvarchar](100) NULL,
	[City] [nvarchar](100) NULL,
	[Region] [nvarchar](100) NULL,
	[Latitude] [decimal](10, 8) NULL,
	[Longitude] [decimal](11, 8) NULL,
	[ISP] [nvarchar](200) NULL,
	[Organization] [nvarchar](200) NULL,
	[Timezone] [nvarchar](100) NULL,
	[FirstVisit] [datetime] NULL,
	[LastVisit] [datetime] NULL,
	[VisitCount] [int] NULL,
	[PagesVisited] [nvarchar](max) NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[VisitorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Admin] ON 

INSERT [dbo].[Admin] ([AdminId], [Username], [Password], [Email], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (1, N'admin', N'admin123', N'admin@portfolio.com', 1, CAST(N'2026-02-28T01:06:15.287' AS DateTime), CAST(N'2026-02-28T01:06:15.287' AS DateTime))
SET IDENTITY_INSERT [dbo].[Admin] OFF
GO
SET IDENTITY_INSERT [dbo].[BlogCategories] ON 

INSERT [dbo].[BlogCategories] ([CategoryId], [CategoryName], [Slug], [Description], [Icon], [DisplayOrder], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (1, N'Technology', N'technology', N'Posts about technology and programming', N'bi-laptop', 0, 1, CAST(N'2026-02-28T01:06:15.310' AS DateTime), CAST(N'2026-02-28T01:06:15.310' AS DateTime))
INSERT [dbo].[BlogCategories] ([CategoryId], [CategoryName], [Slug], [Description], [Icon], [DisplayOrder], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (2, N'Web Development', N'web-development', N'Web development tutorials and tips', N'bi-code-slash', 0, 1, CAST(N'2026-02-28T01:06:15.310' AS DateTime), CAST(N'2026-02-28T01:06:15.310' AS DateTime))
INSERT [dbo].[BlogCategories] ([CategoryId], [CategoryName], [Slug], [Description], [Icon], [DisplayOrder], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (3, N'Career Advice', N'career-advice', N'Career guidance and interview tips okay', N'bi-briefdfndsdcase', 0, 1, CAST(N'2026-02-28T01:06:15.310' AS DateTime), CAST(N'2026-02-28T15:40:18.400' AS DateTime))
INSERT [dbo].[BlogCategories] ([CategoryId], [CategoryName], [Slug], [Description], [Icon], [DisplayOrder], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (4, N'Project Updates', N'project-updates', N'Updates about my projects', N'bi-rocket', 0, 1, CAST(N'2026-02-28T01:06:15.310' AS DateTime), CAST(N'2026-02-28T01:06:15.310' AS DateTime))
INSERT [dbo].[BlogCategories] ([CategoryId], [CategoryName], [Slug], [Description], [Icon], [DisplayOrder], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (5, N'Tutorials', N'tutorials', N'Step by step tutorials', N'bi-book', 0, 1, CAST(N'2026-02-28T01:06:15.310' AS DateTime), CAST(N'2026-02-28T01:06:15.310' AS DateTime))
INSERT [dbo].[BlogCategories] ([CategoryId], [CategoryName], [Slug], [Description], [Icon], [DisplayOrder], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (6, N'sdfsd', N'sdf', N'sdfdfdsfsdaf', N'fa', 0, 0, CAST(N'2026-02-28T15:40:46.383' AS DateTime), CAST(N'2026-02-28T15:41:47.610' AS DateTime))
SET IDENTITY_INSERT [dbo].[BlogCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[BlogComments] ON 

INSERT [dbo].[BlogComments] ([CommentId], [PostId], [ParentCommentId], [VisitorId], [Name], [Email], [Website], [Comment], [IsApproved], [IsSpam], [IPAddress], [LikeCount], [IsActive], [CreatedDate]) VALUES (1, 2, NULL, NULL, N'a', N'asd@gmail.com', N'', N'fyhbhjlo', 0, 0, N'::1', 0, 1, CAST(N'2026-03-01T12:10:40.160' AS DateTime))
INSERT [dbo].[BlogComments] ([CommentId], [PostId], [ParentCommentId], [VisitorId], [Name], [Email], [Website], [Comment], [IsApproved], [IsSpam], [IPAddress], [LikeCount], [IsActive], [CreatedDate]) VALUES (2, 2, NULL, NULL, N'good', N'good@gmail.com', N'', N'ajfdklf', 1, 0, N'::1', 0, 1, CAST(N'2026-03-01T13:15:25.290' AS DateTime))
SET IDENTITY_INSERT [dbo].[BlogComments] OFF
GO
SET IDENTITY_INSERT [dbo].[BlogImages] ON 

INSERT [dbo].[BlogImages] ([ImageId], [PostId], [ImagePath], [ThumbnailPath], [Caption], [AltText], [DisplayOrder], [IsCover], [IsActive], [CreatedDate]) VALUES (1, 1, N'/uploads/blog/images/b3ee0efc-043f-47a1-90df-d821ed0b8156.png', N'', N'', N'', 0, 0, 1, CAST(N'2026-02-28T11:21:44.930' AS DateTime))
INSERT [dbo].[BlogImages] ([ImageId], [PostId], [ImagePath], [ThumbnailPath], [Caption], [AltText], [DisplayOrder], [IsCover], [IsActive], [CreatedDate]) VALUES (2, 2, N'/uploads/blog/images/db8c904c-cd3e-4589-b20a-0281f2abb4a9.png', N'', N'', N'', 0, 0, 1, CAST(N'2026-02-28T11:28:25.983' AS DateTime))
SET IDENTITY_INSERT [dbo].[BlogImages] OFF
GO
SET IDENTITY_INSERT [dbo].[BlogLikes] ON 

INSERT [dbo].[BlogLikes] ([LikeId], [PostId], [VisitorId], [IPAddress], [CreatedDate]) VALUES (8, 2, 36787, N'::1', CAST(N'2026-03-01T12:58:12.060' AS DateTime))
INSERT [dbo].[BlogLikes] ([LikeId], [PostId], [VisitorId], [IPAddress], [CreatedDate]) VALUES (9, 2, 1, N'::1', CAST(N'2026-03-01T13:22:27.680' AS DateTime))
INSERT [dbo].[BlogLikes] ([LikeId], [PostId], [VisitorId], [IPAddress], [CreatedDate]) VALUES (10, 2, 2, N'::1', CAST(N'2026-03-01T13:22:31.847' AS DateTime))
INSERT [dbo].[BlogLikes] ([LikeId], [PostId], [VisitorId], [IPAddress], [CreatedDate]) VALUES (11, 2, 3, N'::1', CAST(N'2026-03-01T13:25:16.210' AS DateTime))
INSERT [dbo].[BlogLikes] ([LikeId], [PostId], [VisitorId], [IPAddress], [CreatedDate]) VALUES (12, 2, 4, N'::1', CAST(N'2026-03-01T13:25:17.897' AS DateTime))
INSERT [dbo].[BlogLikes] ([LikeId], [PostId], [VisitorId], [IPAddress], [CreatedDate]) VALUES (13, 2, 5, N'::1', CAST(N'2026-03-01T14:09:11.767' AS DateTime))
INSERT [dbo].[BlogLikes] ([LikeId], [PostId], [VisitorId], [IPAddress], [CreatedDate]) VALUES (14, 2, 6, N'::1', CAST(N'2026-03-04T09:43:07.357' AS DateTime))
SET IDENTITY_INSERT [dbo].[BlogLikes] OFF
GO
SET IDENTITY_INSERT [dbo].[BlogPosts] ON 

INSERT [dbo].[BlogPosts] ([PostId], [CategoryId], [Title], [Slug], [Excerpt], [Content], [FeaturedImage], [VideoUrl], [VideoEmbedCode], [Tags], [MetaTitle], [MetaDescription], [MetaKeywords], [ViewCount], [LikeCount], [CommentCount], [IsPublished], [PublishedDate], [IsFeatured], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (1, 3, N'fgfdsgbs', N'fgfgdfgsd', N'sgfbfb', N'<h2>dfksdjflsd</h2>df<h3>dfjsdjkf</h3><b>text</b><b>text</b><i>text</i><u>dfjsdfkdf<ul>
</ul></u><img src="https://" alt="Image" class="img-fluid" style="max-width:100%;"><pre><code>// Your code here</code></pre><p>text</p>', N'/uploads/blog/featured/2d49d5e1-00ab-4139-942d-ec987576728c.png', N'dvkdxcv', N'xcvxcvmm', N'dfgdfbgsf', N'fgdfkolg', N'fgbdfbg', N'fgsdfgsfgdsf', 0, 0, 0, 1, NULL, 1, 0, CAST(N'2026-02-28T11:21:44.527' AS DateTime), CAST(N'2026-02-28T11:22:40.237' AS DateTime))
INSERT [dbo].[BlogPosts] ([PostId], [CategoryId], [Title], [Slug], [Excerpt], [Content], [FeaturedImage], [VideoUrl], [VideoEmbedCode], [Tags], [MetaTitle], [MetaDescription], [MetaKeywords], [ViewCount], [LikeCount], [CommentCount], [IsPublished], [PublishedDate], [IsFeatured], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (2, 4, N'Create Blog Post', N'create-blog-post', N'Create Blog Post', N'<!DOCTYPE html>
<html>
<head>
    <title>My Blog</title>

    <style>
        body {
            background-color: #f4f6f9;
            font-family: Arial, sans-serif;
            margin: 40px;
            line-height: 1.6;
        }

        h1 {
            color: #2c3e50;
            text-align: center;
        }

        h2 {
            color: #34495e;
            margin-top: 30px;
        }

        p {
            font-size: 18px;
            color: #555;
        }
    </style>
</head>

<body>

    <h1>My Learning Journey in Web Development</h1>

    <h2>Introduction</h2>
    <p>
        Hello everyone! I am currently learning HTML and CSS.
        It is very interesting to create web pages using simple code.
    </p>

    <h2>Why I Love Coding</h2>
    <p>
        Coding helps me to build creative projects.
        I want to become a professional Full Stack Developer.
    </p>

    <h2>My Future Goals</h2>
    <p>
        I am improving my programming skills every day.
        Soon I will start working on real-world projects.
    </p>

</body>
</html>', N'/uploads/blog/featured/11879627-7777-456d-bfd9-5682cef1d7c4.png', N'ffgfdgf', N'fdgdsfg', N'fgsdfgv', N'fgsdfg', N'sfgsdf', N'fgsd', 14, 7, 0, 1, NULL, 1, 1, CAST(N'2026-02-28T11:28:25.767' AS DateTime), CAST(N'2026-02-28T11:28:25.767' AS DateTime))
SET IDENTITY_INSERT [dbo].[BlogPosts] OFF
GO
SET IDENTITY_INSERT [dbo].[BlogVideos] ON 

INSERT [dbo].[BlogVideos] ([VideoId], [PostId], [VideoTitle], [VideoUrl], [VideoEmbedCode], [VideoType], [ThumbnailPath], [DisplayOrder], [IsActive], [CreatedDate]) VALUES (1, 1, N'fgfdsgbs', N'/uploads/blog/videos/3aa701e7-a672-4c54-8c57-2e085403c8e2.mp4', NULL, N'MP4', N'', 0, 1, CAST(N'2026-02-28T11:21:45.053' AS DateTime))
INSERT [dbo].[BlogVideos] ([VideoId], [PostId], [VideoTitle], [VideoUrl], [VideoEmbedCode], [VideoType], [ThumbnailPath], [DisplayOrder], [IsActive], [CreatedDate]) VALUES (2, 2, N'Create Blog Post', N'/uploads/blog/videos/a10bc68d-b66d-491e-a661-ec69f432af91.mp4', NULL, N'MP4', N'', 0, 1, CAST(N'2026-02-28T11:28:26.080' AS DateTime))
SET IDENTITY_INSERT [dbo].[BlogVideos] OFF
GO
SET IDENTITY_INSERT [dbo].[Education] ON 

INSERT [dbo].[Education] ([EducationId], [Degree], [Institute], [Year], [Percentage], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (1, N'BA', N'GPU', 2020, CAST(79.00 AS Decimal(5, 2)), 0, CAST(N'2026-02-28T13:56:50.270' AS DateTime), CAST(N'2026-02-28T13:57:16.527' AS DateTime))
INSERT [dbo].[Education] ([EducationId], [Degree], [Institute], [Year], [Percentage], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (2, N'BCS', N'DSFKSDLJ', 2020, CAST(100.00 AS Decimal(5, 2)), 1, CAST(N'2026-02-28T17:55:56.867' AS DateTime), CAST(N'2026-02-28T17:55:56.867' AS DateTime))
SET IDENTITY_INSERT [dbo].[Education] OFF
GO
SET IDENTITY_INSERT [dbo].[Experience] ON 

INSERT [dbo].[Experience] ([ExperienceId], [CompanyName], [Role], [StartDate], [EndDate], [Description], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (1, N'wejk', N'FULL STACK', CAST(N'2026-02-01' AS Date), CAST(N'2026-02-28' AS Date), N'YUGHOJDZJHILVCFGD   HUKJHKJK', 0, CAST(N'2026-02-28T14:10:36.360' AS DateTime), CAST(N'2026-02-28T14:10:55.700' AS DateTime))
INSERT [dbo].[Experience] ([ExperienceId], [CompanyName], [Role], [StartDate], [EndDate], [Description], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (2, N'BCA', N'FULL STACK', CAST(N'2026-02-28' AS Date), CAST(N'2026-02-28' AS Date), N'DSFIJJ;KKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK', 1, CAST(N'2026-02-28T17:55:31.540' AS DateTime), CAST(N'2026-02-28T17:55:31.540' AS DateTime))
SET IDENTITY_INSERT [dbo].[Experience] OFF
GO
SET IDENTITY_INSERT [dbo].[Gallery] ON 

INSERT [dbo].[Gallery] ([GalleryId], [Title], [Description], [MediaType], [MediaPath], [ThumbnailPath], [VideoEmbedCode], [Category], [Tags], [DisplayOrder], [IsFeatured], [ViewCount], [DownloadCount], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (1, N'ABC', N'Main tumhe complete corrected', N'', N'', N'', N'https://www.youtube.com/watch?v=Aa5gV3o7Rno&t=1819s', N'ASP', N'ASP', 1, 1, 0, 0, 1, CAST(N'2026-02-28T16:28:13.110' AS DateTime), CAST(N'2026-02-28T16:55:32.473' AS DateTime))
INSERT [dbo].[Gallery] ([GalleryId], [Title], [Description], [MediaType], [MediaPath], [ThumbnailPath], [VideoEmbedCode], [Category], [Tags], [DisplayOrder], [IsFeatured], [ViewCount], [DownloadCount], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (2, N'ASP', N'BJU', N'', N'', N'', N'', N'ASP', N'dfgdfbgsf', 1, 1, 0, 0, 1, CAST(N'2026-02-28T16:29:22.840' AS DateTime), CAST(N'2026-02-28T16:38:03.197' AS DateTime))
INSERT [dbo].[Gallery] ([GalleryId], [Title], [Description], [MediaType], [MediaPath], [ThumbnailPath], [VideoEmbedCode], [Category], [Tags], [DisplayOrder], [IsFeatured], [ViewCount], [DownloadCount], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (3, N'Gallery Item', N'GHHJ', N'', N'', N'', N'', N'', N'', 0, 0, 1, 0, 1, CAST(N'2026-02-28T16:56:50.077' AS DateTime), CAST(N'2026-02-28T23:02:10.803' AS DateTime))
SET IDENTITY_INSERT [dbo].[Gallery] OFF
GO
SET IDENTITY_INSERT [dbo].[Profile] ON 

INSERT [dbo].[Profile] ([ProfileId], [Name], [Title], [Description], [Email], [Phone], [Address], [LinkedIn], [GitHub], [Photo], [ResumePath], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (1, N'A', N'B', N'DFDF', N'zadfg@gmail.com', N'4657456789', N'fdfdf', N'', N'', N'/uploads/photos/92235c45-473a-4393-94b7-45d95843dc08.png', N'', 1, CAST(N'2026-02-28T09:38:43.517' AS DateTime), CAST(N'2026-03-04T09:44:56.457' AS DateTime))
SET IDENTITY_INSERT [dbo].[Profile] OFF
GO
SET IDENTITY_INSERT [dbo].[Projects] ON 

INSERT [dbo].[Projects] ([ProjectId], [ProjectName], [Description], [ImagePath], [GitHubLink], [LiveLink], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (1, N'WCS CAPITAL', N'Fine Technologies has expertise in Binary Income Plan, Australian Binary Plan, Tri Binary, Spill over Income Plan, Matrix Plan, Board Plan, Growth Plan and custom compensation plan and many more....

Fine Technologies MLM Software in Delhi offers complete website design and integrated services in-house that suits to a smart mlm software business. We are complete-service partner, you don''t have to coordinate and manage your website development and maintenance with multiple vendors. This makes your life easier and your technology work smoother. Our goal is not only to create an incredible and user friendly website for your company, but also to help you stand out as an incredible MLM business.', N'/uploads/projects/a86393e0-f01a-4a8f-9e0b-63251b293c42.png', N'https://localhost:44341/Admin/Projects', N'https://localhost:44341/Admin/Projects', 1, CAST(N'2026-02-28T10:20:43.887' AS DateTime), CAST(N'2026-02-28T10:20:43.887' AS DateTime))
SET IDENTITY_INSERT [dbo].[Projects] OFF
GO
SET IDENTITY_INSERT [dbo].[ResumeViews] ON 

INSERT [dbo].[ResumeViews] ([ViewId], [VisitorName], [VisitorEmail], [CompanyName], [Designation], [ViewDate], [IPAddress], [UserAgent], [Country], [City], [IsDownloaded], [DownloadDate], [IsActive], [CreatedDate]) VALUES (1, N'A', N'aj', N'wejk', N'asp.net', CAST(N'2026-02-28T04:08:33.843' AS DateTime), N'::1', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36', N'', N'', 1, CAST(N'2026-02-28T04:08:33.843' AS DateTime), 1, CAST(N'2026-02-28T04:08:34.100' AS DateTime))
INSERT [dbo].[ResumeViews] ([ViewId], [VisitorName], [VisitorEmail], [CompanyName], [Designation], [ViewDate], [IPAddress], [UserAgent], [Country], [City], [IsDownloaded], [DownloadDate], [IsActive], [CreatedDate]) VALUES (2, N'A', N'aj@gmail.com', N'wejk', N'asp.net', CAST(N'2026-02-28T10:42:50.067' AS DateTime), N'::1', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36', N'', N'', 1, CAST(N'2026-02-28T10:42:50.067' AS DateTime), 1, CAST(N'2026-02-28T10:42:50.110' AS DateTime))
INSERT [dbo].[ResumeViews] ([ViewId], [VisitorName], [VisitorEmail], [CompanyName], [Designation], [ViewDate], [IPAddress], [UserAgent], [Country], [City], [IsDownloaded], [DownloadDate], [IsActive], [CreatedDate]) VALUES (3, N'qww', N'ast@gamil.com', N'dfd', N'fdfd', CAST(N'2026-02-28T18:06:50.937' AS DateTime), N'::1', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36', N'', N'', 1, CAST(N'2026-02-28T18:06:50.937' AS DateTime), 1, CAST(N'2026-02-28T18:06:50.993' AS DateTime))
INSERT [dbo].[ResumeViews] ([ViewId], [VisitorName], [VisitorEmail], [CompanyName], [Designation], [ViewDate], [IPAddress], [UserAgent], [Country], [City], [IsDownloaded], [DownloadDate], [IsActive], [CreatedDate]) VALUES (4, N'dkf', N'Asdjfd@gmail.com', N'', N'', CAST(N'2026-02-28T22:52:01.200' AS DateTime), N'::1', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36', N'', N'', 1, CAST(N'2026-02-28T22:52:01.200' AS DateTime), 1, CAST(N'2026-02-28T22:52:01.427' AS DateTime))
INSERT [dbo].[ResumeViews] ([ViewId], [VisitorName], [VisitorEmail], [CompanyName], [Designation], [ViewDate], [IPAddress], [UserAgent], [Country], [City], [IsDownloaded], [DownloadDate], [IsActive], [CreatedDate]) VALUES (5, N'hgf', N'Asdjfd@gmail.com', N'BCA', N'asp.net', CAST(N'2026-03-01T14:25:16.510' AS DateTime), N'::1', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36', N'India', N'Mumbai', 1, CAST(N'2026-03-01T14:25:16.513' AS DateTime), 1, CAST(N'2026-03-01T14:25:16.513' AS DateTime))
INSERT [dbo].[ResumeViews] ([ViewId], [VisitorName], [VisitorEmail], [CompanyName], [Designation], [ViewDate], [IPAddress], [UserAgent], [Country], [City], [IsDownloaded], [DownloadDate], [IsActive], [CreatedDate]) VALUES (6, N'dkf', N'aj0@gmail.com', N'wejk', N'asp.net', CAST(N'2026-03-01T14:41:31.753' AS DateTime), N'::1', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36', N'India', N'Mumbai', 1, CAST(N'2026-03-01T14:41:31.757' AS DateTime), 1, CAST(N'2026-03-01T14:41:31.757' AS DateTime))
SET IDENTITY_INSERT [dbo].[ResumeViews] OFF
GO
SET IDENTITY_INSERT [dbo].[Skills] ON 

INSERT [dbo].[Skills] ([SkillId], [SkillName], [Percentage], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (1, N'ASP.NET', 67, 1, CAST(N'2026-02-28T10:17:10.303' AS DateTime), CAST(N'2026-02-28T10:17:10.303' AS DateTime))
INSERT [dbo].[Skills] ([SkillId], [SkillName], [Percentage], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (2, N'PHP', 100, 1, CAST(N'2026-02-28T22:58:56.743' AS DateTime), CAST(N'2026-02-28T22:58:56.743' AS DateTime))
INSERT [dbo].[Skills] ([SkillId], [SkillName], [Percentage], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (3, N'ADO.NET', 100, 1, CAST(N'2026-03-04T09:45:25.407' AS DateTime), CAST(N'2026-03-04T09:45:25.407' AS DateTime))
SET IDENTITY_INSERT [dbo].[Skills] OFF
GO
SET IDENTITY_INSERT [dbo].[VisitorTracking] ON 

INSERT [dbo].[VisitorTracking] ([VisitorId], [SessionId], [IPAddress], [UserAgent], [Country], [City], [Region], [Latitude], [Longitude], [ISP], [Organization], [Timezone], [FirstVisit], [LastVisit], [VisitCount], [PagesVisited], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (1, N'9282661b-10e6-0fad-2c8b-684c7599bb64', N'::1', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2026-03-01T13:22:27.543' AS DateTime), CAST(N'2026-03-01T13:22:27.543' AS DateTime), 1, NULL, 1, CAST(N'2026-03-01T13:22:27.597' AS DateTime), CAST(N'2026-03-01T13:22:27.597' AS DateTime))
INSERT [dbo].[VisitorTracking] ([VisitorId], [SessionId], [IPAddress], [UserAgent], [Country], [City], [Region], [Latitude], [Longitude], [ISP], [Organization], [Timezone], [FirstVisit], [LastVisit], [VisitCount], [PagesVisited], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (2, N'9282661b-10e6-0fad-2c8b-684c7599bb64', N'::1', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2026-03-01T13:22:31.830' AS DateTime), CAST(N'2026-03-01T13:22:31.830' AS DateTime), 1, NULL, 1, CAST(N'2026-03-01T13:22:31.837' AS DateTime), CAST(N'2026-03-01T13:22:31.837' AS DateTime))
INSERT [dbo].[VisitorTracking] ([VisitorId], [SessionId], [IPAddress], [UserAgent], [Country], [City], [Region], [Latitude], [Longitude], [ISP], [Organization], [Timezone], [FirstVisit], [LastVisit], [VisitCount], [PagesVisited], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (3, N'9282661b-10e6-0fad-2c8b-684c7599bb64', N'::1', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2026-03-01T13:25:16.143' AS DateTime), CAST(N'2026-03-01T13:25:16.143' AS DateTime), 1, NULL, 1, CAST(N'2026-03-01T13:25:16.177' AS DateTime), CAST(N'2026-03-01T13:25:16.177' AS DateTime))
INSERT [dbo].[VisitorTracking] ([VisitorId], [SessionId], [IPAddress], [UserAgent], [Country], [City], [Region], [Latitude], [Longitude], [ISP], [Organization], [Timezone], [FirstVisit], [LastVisit], [VisitCount], [PagesVisited], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (4, N'9282661b-10e6-0fad-2c8b-684c7599bb64', N'::1', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2026-03-01T13:25:17.857' AS DateTime), CAST(N'2026-03-01T13:25:17.857' AS DateTime), 1, NULL, 1, CAST(N'2026-03-01T13:25:17.860' AS DateTime), CAST(N'2026-03-01T13:25:17.860' AS DateTime))
INSERT [dbo].[VisitorTracking] ([VisitorId], [SessionId], [IPAddress], [UserAgent], [Country], [City], [Region], [Latitude], [Longitude], [ISP], [Organization], [Timezone], [FirstVisit], [LastVisit], [VisitCount], [PagesVisited], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (5, N'a2fa7751-8526-224c-0ba2-294a2d16984e', N'::1', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2026-03-01T14:09:11.550' AS DateTime), CAST(N'2026-03-01T14:09:11.550' AS DateTime), 1, NULL, 1, CAST(N'2026-03-01T14:09:11.630' AS DateTime), CAST(N'2026-03-01T14:09:11.630' AS DateTime))
INSERT [dbo].[VisitorTracking] ([VisitorId], [SessionId], [IPAddress], [UserAgent], [Country], [City], [Region], [Latitude], [Longitude], [ISP], [Organization], [Timezone], [FirstVisit], [LastVisit], [VisitCount], [PagesVisited], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (6, N'198102e5-76bb-3be3-d8ea-0a0cac429dee', N'::1', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2026-03-04T09:43:07.253' AS DateTime), CAST(N'2026-03-04T09:43:07.253' AS DateTime), 1, NULL, 1, CAST(N'2026-03-04T09:43:07.303' AS DateTime), CAST(N'2026-03-04T09:43:07.303' AS DateTime))
SET IDENTITY_INSERT [dbo].[VisitorTracking] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__BlogCate__BC7B5FB673EB32C9]    Script Date: 08-03-2026 21:55:30 ******/
ALTER TABLE [dbo].[BlogCategories] ADD UNIQUE NONCLUSTERED 
(
	[Slug] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__BlogPost__BC7B5FB6C8C95897]    Script Date: 08-03-2026 21:55:30 ******/
ALTER TABLE [dbo].[BlogPosts] ADD UNIQUE NONCLUSTERED 
(
	[Slug] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__GalleryA__BC7B5FB6B5BA5020]    Script Date: 08-03-2026 21:55:30 ******/
ALTER TABLE [dbo].[GalleryAlbums] ADD UNIQUE NONCLUSTERED 
(
	[Slug] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Admin] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Admin] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Admin] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[AlbumItems] ADD  DEFAULT ((0)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[AlbumItems] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[BlogCategories] ADD  DEFAULT ((0)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[BlogCategories] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[BlogCategories] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[BlogCategories] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[BlogComments] ADD  DEFAULT ((0)) FOR [IsApproved]
GO
ALTER TABLE [dbo].[BlogComments] ADD  DEFAULT ((0)) FOR [IsSpam]
GO
ALTER TABLE [dbo].[BlogComments] ADD  DEFAULT ((0)) FOR [LikeCount]
GO
ALTER TABLE [dbo].[BlogComments] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[BlogComments] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[BlogImages] ADD  DEFAULT ((0)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[BlogImages] ADD  DEFAULT ((0)) FOR [IsCover]
GO
ALTER TABLE [dbo].[BlogImages] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[BlogImages] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[BlogLikes] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[BlogPosts] ADD  DEFAULT ((0)) FOR [ViewCount]
GO
ALTER TABLE [dbo].[BlogPosts] ADD  DEFAULT ((0)) FOR [LikeCount]
GO
ALTER TABLE [dbo].[BlogPosts] ADD  DEFAULT ((0)) FOR [CommentCount]
GO
ALTER TABLE [dbo].[BlogPosts] ADD  DEFAULT ((1)) FOR [IsPublished]
GO
ALTER TABLE [dbo].[BlogPosts] ADD  DEFAULT ((0)) FOR [IsFeatured]
GO
ALTER TABLE [dbo].[BlogPosts] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[BlogPosts] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[BlogPosts] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[BlogVideos] ADD  DEFAULT ((0)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[BlogVideos] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[BlogVideos] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ChatMessages] ADD  DEFAULT ((0)) FOR [IsFromAdmin]
GO
ALTER TABLE [dbo].[ChatMessages] ADD  DEFAULT ((0)) FOR [IsRead]
GO
ALTER TABLE [dbo].[ChatMessages] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ChatMessages] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ContactMessages] ADD  DEFAULT ((0)) FOR [IsRead]
GO
ALTER TABLE [dbo].[ContactMessages] ADD  DEFAULT ((0)) FOR [IsReplied]
GO
ALTER TABLE [dbo].[ContactMessages] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ContactMessages] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ContactMessages] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Education] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Education] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Education] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Experience] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Experience] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Experience] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Gallery] ADD  DEFAULT ((0)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[Gallery] ADD  DEFAULT ((0)) FOR [IsFeatured]
GO
ALTER TABLE [dbo].[Gallery] ADD  DEFAULT ((0)) FOR [ViewCount]
GO
ALTER TABLE [dbo].[Gallery] ADD  DEFAULT ((0)) FOR [DownloadCount]
GO
ALTER TABLE [dbo].[Gallery] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Gallery] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Gallery] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[GalleryAlbums] ADD  DEFAULT ((0)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[GalleryAlbums] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[GalleryAlbums] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[GalleryAlbums] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[PageVisits] ADD  DEFAULT (getdate()) FOR [VisitTime]
GO
ALTER TABLE [dbo].[PageVisits] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[PageVisits] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Profile] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Profile] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Profile] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Projects] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Projects] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Projects] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[ResumeViews] ADD  DEFAULT (getdate()) FOR [ViewDate]
GO
ALTER TABLE [dbo].[ResumeViews] ADD  DEFAULT ((0)) FOR [IsDownloaded]
GO
ALTER TABLE [dbo].[ResumeViews] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ResumeViews] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Skills] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Skills] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Skills] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[VisitorTracking] ADD  DEFAULT (getdate()) FOR [FirstVisit]
GO
ALTER TABLE [dbo].[VisitorTracking] ADD  DEFAULT (getdate()) FOR [LastVisit]
GO
ALTER TABLE [dbo].[VisitorTracking] ADD  DEFAULT ((1)) FOR [VisitCount]
GO
ALTER TABLE [dbo].[VisitorTracking] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[VisitorTracking] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[VisitorTracking] ADD  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[AlbumItems]  WITH CHECK ADD FOREIGN KEY([AlbumId])
REFERENCES [dbo].[GalleryAlbums] ([AlbumId])
GO
ALTER TABLE [dbo].[AlbumItems]  WITH CHECK ADD FOREIGN KEY([GalleryId])
REFERENCES [dbo].[Gallery] ([GalleryId])
GO
ALTER TABLE [dbo].[BlogComments]  WITH CHECK ADD FOREIGN KEY([PostId])
REFERENCES [dbo].[BlogPosts] ([PostId])
GO
ALTER TABLE [dbo].[BlogComments]  WITH CHECK ADD FOREIGN KEY([VisitorId])
REFERENCES [dbo].[VisitorTracking] ([VisitorId])
GO
ALTER TABLE [dbo].[BlogImages]  WITH CHECK ADD FOREIGN KEY([PostId])
REFERENCES [dbo].[BlogPosts] ([PostId])
GO
ALTER TABLE [dbo].[BlogPosts]  WITH CHECK ADD FOREIGN KEY([CategoryId])
REFERENCES [dbo].[BlogCategories] ([CategoryId])
GO
ALTER TABLE [dbo].[BlogVideos]  WITH CHECK ADD FOREIGN KEY([PostId])
REFERENCES [dbo].[BlogPosts] ([PostId])
GO
ALTER TABLE [dbo].[ChatMessages]  WITH CHECK ADD FOREIGN KEY([VisitorId])
REFERENCES [dbo].[VisitorTracking] ([VisitorId])
GO
ALTER TABLE [dbo].[PageVisits]  WITH CHECK ADD FOREIGN KEY([VisitorId])
REFERENCES [dbo].[VisitorTracking] ([VisitorId])
GO
ALTER TABLE [dbo].[Skills]  WITH CHECK ADD CHECK  (([Percentage]>=(0) AND [Percentage]<=(100)))
GO
/****** Object:  StoredProcedure [dbo].[sp_AddAdmin]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Add New Admin
CREATE   PROCEDURE [dbo].[sp_AddAdmin]
    @Username NVARCHAR(50),
    @Password NVARCHAR(255),
    @Email NVARCHAR(100)
AS
BEGIN
    INSERT INTO Admin (Username, Password, Email, IsActive, CreatedDate, UpdatedDate)
    VALUES (@Username, @Password, @Email, 1, GETDATE(), GETDATE());
    
    SELECT SCOPE_IDENTITY() AS AdminId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddBlogCategory]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Add Blog Category
CREATE   PROCEDURE [dbo].[sp_AddBlogCategory]
    @CategoryName NVARCHAR(100),
    @Slug NVARCHAR(100),
    @Description NVARCHAR(500),
    @Icon NVARCHAR(100),
    @DisplayOrder INT
AS
BEGIN
    INSERT INTO BlogCategories (CategoryName, Slug, Description, Icon, DisplayOrder, IsActive, CreatedDate, UpdatedDate)
    VALUES (@CategoryName, @Slug, @Description, @Icon, @DisplayOrder, 1, GETDATE(), GETDATE());
    
    SELECT SCOPE_IDENTITY() AS CategoryId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddBlogComment]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================== BLOG COMMENTS STORED PROCEDURES ====================

-- Add Blog Comment
CREATE   PROCEDURE [dbo].[sp_AddBlogComment]
    @PostId INT,
    @ParentCommentId INT,
    @VisitorId INT,
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @Website NVARCHAR(500),
    @Comment NVARCHAR(MAX),
    @IPAddress NVARCHAR(50)
AS
BEGIN
    INSERT INTO BlogComments (PostId, ParentCommentId, VisitorId, Name, Email, Website, Comment, IPAddress, IsActive, CreatedDate)
    VALUES (@PostId, @ParentCommentId, @VisitorId, @Name, @Email, @Website, @Comment, @IPAddress, 1, GETDATE());
    
    -- Update comment count
    UPDATE BlogPosts SET CommentCount = CommentCount + 1 WHERE PostId = @PostId;
    
    SELECT SCOPE_IDENTITY() AS CommentId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddBlogImage]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================== BLOG IMAGES STORED PROCEDURES ====================

-- Add Blog Image
CREATE   PROCEDURE [dbo].[sp_AddBlogImage]
    @PostId INT,
    @ImagePath NVARCHAR(500),
    @ThumbnailPath NVARCHAR(500),
    @Caption NVARCHAR(500),
    @AltText NVARCHAR(200),
    @DisplayOrder INT,
    @IsCover BIT
AS
BEGIN
    -- If this is cover image, remove cover from others
    IF @IsCover = 1
    BEGIN
        UPDATE BlogImages SET IsCover = 0 WHERE PostId = @PostId;
    END
    
    INSERT INTO BlogImages (PostId, ImagePath, ThumbnailPath, Caption, AltText, DisplayOrder, IsCover, IsActive, CreatedDate)
    VALUES (@PostId, @ImagePath, @ThumbnailPath, @Caption, @AltText, @DisplayOrder, @IsCover, 1, GETDATE());
    
    SELECT SCOPE_IDENTITY() AS ImageId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddBlogPost]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Add Blog Post
CREATE   PROCEDURE [dbo].[sp_AddBlogPost]
    @CategoryId INT,
    @Title NVARCHAR(500),
    @Slug NVARCHAR(500),
    @Excerpt NVARCHAR(1000),
    @Content NVARCHAR(MAX),
    @FeaturedImage NVARCHAR(500),
    @VideoUrl NVARCHAR(500),
    @VideoEmbedCode NVARCHAR(MAX),
    @Tags NVARCHAR(500),
    @MetaTitle NVARCHAR(200),
    @MetaDescription NVARCHAR(500),
    @MetaKeywords NVARCHAR(500),
    @IsPublished BIT,
    @PublishedDate DATETIME,
    @IsFeatured BIT
AS
BEGIN
    INSERT INTO BlogPosts (
        CategoryId, Title, Slug, Excerpt, Content, FeaturedImage, 
        VideoUrl, VideoEmbedCode, Tags, MetaTitle, MetaDescription, 
        MetaKeywords, IsPublished, PublishedDate, IsFeatured, 
        IsActive, CreatedDate, UpdatedDate
    )
    VALUES (
        @CategoryId, @Title, @Slug, @Excerpt, @Content, @FeaturedImage,
        @VideoUrl, @VideoEmbedCode, @Tags, @MetaTitle, @MetaDescription,
        @MetaKeywords, @IsPublished, @PublishedDate, @IsFeatured,
        1, GETDATE(), GETDATE()
    );
    
    SELECT SCOPE_IDENTITY() AS PostId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddEducation]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Add New Education
CREATE   PROCEDURE [dbo].[sp_AddEducation]
    @Degree NVARCHAR(100),
    @Institute NVARCHAR(200),
    @Year INT,
    @Percentage DECIMAL(5,2)
AS
BEGIN
    INSERT INTO Education (Degree, Institute, Year, Percentage, IsActive, CreatedDate, UpdatedDate)
    VALUES (@Degree, @Institute, @Year, @Percentage, 1, GETDATE(), GETDATE());
    
    SELECT SCOPE_IDENTITY() AS EducationId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddExperience]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Add New Experience
CREATE   PROCEDURE [dbo].[sp_AddExperience]
    @CompanyName NVARCHAR(200),
    @Role NVARCHAR(100),
    @StartDate DATE,
    @EndDate DATE,
    @Description NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Experience (CompanyName, Role, StartDate, EndDate, Description, IsActive, CreatedDate, UpdatedDate)
    VALUES (@CompanyName, @Role, @StartDate, @EndDate, @Description, 1, GETDATE(), GETDATE());
    
    SELECT SCOPE_IDENTITY() AS ExperienceId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddGalleryItem]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Add Gallery Item
CREATE   PROCEDURE [dbo].[sp_AddGalleryItem]
    @Title NVARCHAR(500),
    @Description NVARCHAR(MAX),
    @MediaType NVARCHAR(50),
    @MediaPath NVARCHAR(500),
    @ThumbnailPath NVARCHAR(500),
    @VideoEmbedCode NVARCHAR(MAX),
    @Category NVARCHAR(100),
    @Tags NVARCHAR(500),
    @DisplayOrder INT,
    @IsFeatured BIT
AS
BEGIN
    INSERT INTO Gallery (Title, Description, MediaType, MediaPath, ThumbnailPath, VideoEmbedCode, Category, Tags, DisplayOrder, IsFeatured, IsActive, CreatedDate, UpdatedDate)
    VALUES (@Title, @Description, @MediaType, @MediaPath, @ThumbnailPath, @VideoEmbedCode, @Category, @Tags, @DisplayOrder, @IsFeatured, 1, GETDATE(), GETDATE());
    
    SELECT SCOPE_IDENTITY() AS GalleryId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddProject]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Add New Project
CREATE   PROCEDURE [dbo].[sp_AddProject]
    @ProjectName NVARCHAR(200),
    @Description NVARCHAR(MAX),
    @ImagePath NVARCHAR(255),
    @GitHubLink NVARCHAR(500),
    @LiveLink NVARCHAR(500)
AS
BEGIN
    INSERT INTO Projects (ProjectName, Description, ImagePath, GitHubLink, LiveLink, IsActive, CreatedDate, UpdatedDate)
    VALUES (@ProjectName, @Description, @ImagePath, @GitHubLink, @LiveLink, 1, GETDATE(), GETDATE());
    
    SELECT SCOPE_IDENTITY() AS ProjectId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddSkill]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Add New Skill
CREATE   PROCEDURE [dbo].[sp_AddSkill]
    @SkillName NVARCHAR(100),
    @Percentage INT
AS
BEGIN
    INSERT INTO Skills (SkillName, Percentage, IsActive, CreatedDate, UpdatedDate)
    VALUES (@SkillName, @Percentage, 1, GETDATE(), GETDATE());
    
    SELECT SCOPE_IDENTITY() AS SkillId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AdminLogin]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==================== ADMIN STORED PROCEDURES ====================

-- Admin Login
CREATE   PROCEDURE [dbo].[sp_AdminLogin]
    @Username NVARCHAR(50),
    @Password NVARCHAR(255)
AS
BEGIN
    SELECT AdminId, Username, Email, IsActive, CreatedDate 
    FROM Admin 
    WHERE Username = @Username AND Password = @Password AND IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ApproveComment]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Approve Comment
CREATE   PROCEDURE [dbo].[sp_ApproveComment]
    @CommentId INT
AS
BEGIN
    UPDATE BlogComments SET IsApproved = 1 WHERE CommentId = @CommentId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CheckUserLike]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Check if User Liked Post
CREATE   PROCEDURE [dbo].[sp_CheckUserLike]
    @PostId INT,
    @VisitorId INT
AS
BEGIN
    SELECT CASE WHEN EXISTS(SELECT 1 FROM BlogLikes WHERE PostId = @PostId AND VisitorId = @VisitorId) 
           THEN 1 ELSE 0 END AS HasLiked;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteAdmin]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Delete Admin (Soft Delete)
CREATE   PROCEDURE [dbo].[sp_DeleteAdmin]
    @AdminId INT
AS
BEGIN
    UPDATE Admin 
    SET IsActive = 0, UpdatedDate = GETDATE()
    WHERE AdminId = @AdminId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteBlogCategory]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Delete Blog Category
CREATE   PROCEDURE [dbo].[sp_DeleteBlogCategory]
    @CategoryId INT
AS
BEGIN
    UPDATE BlogCategories 
    SET IsActive = 0, UpdatedDate = GETDATE()
    WHERE CategoryId = @CategoryId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteBlogImage]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Delete Blog Image
CREATE   PROCEDURE [dbo].[sp_DeleteBlogImage]
    @ImageId INT
AS
BEGIN
    UPDATE BlogImages SET IsActive = 0 WHERE ImageId = @ImageId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteBlogPost]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Delete Blog Post (Soft Delete)
CREATE   PROCEDURE [dbo].[sp_DeleteBlogPost]
    @PostId INT
AS
BEGIN
    UPDATE BlogPosts SET IsActive = 0, UpdatedDate = GETDATE() WHERE PostId = @PostId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteComment]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Delete Comment
CREATE   PROCEDURE [dbo].[sp_DeleteComment]
    @CommentId INT
AS
BEGIN
    UPDATE BlogComments SET IsActive = 0 WHERE CommentId = @CommentId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteEducation]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Delete Education (Soft Delete)
CREATE   PROCEDURE [dbo].[sp_DeleteEducation]
    @EducationId INT
AS
BEGIN
    UPDATE Education 
    SET IsActive = 0, UpdatedDate = GETDATE()
    WHERE EducationId = @EducationId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteExperience]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Delete Experience (Soft Delete)
CREATE   PROCEDURE [dbo].[sp_DeleteExperience]
    @ExperienceId INT
AS
BEGIN
    UPDATE Experience 
    SET IsActive = 0, UpdatedDate = GETDATE()
    WHERE ExperienceId = @ExperienceId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteGalleryItem]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Delete Gallery Item
CREATE   PROCEDURE [dbo].[sp_DeleteGalleryItem]
    @GalleryId INT
AS
BEGIN
    UPDATE Gallery SET IsActive = 0, UpdatedDate = GETDATE() WHERE GalleryId = @GalleryId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteMessage]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Delete Message (Soft Delete)
CREATE   PROCEDURE [dbo].[sp_DeleteMessage]
    @MessageId INT
AS
BEGIN
    UPDATE ContactMessages 
    SET IsActive = 0, UpdatedDate = GETDATE()
    WHERE MessageId = @MessageId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteProject]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Delete Project (Soft Delete)
CREATE   PROCEDURE [dbo].[sp_DeleteProject]
    @ProjectId INT
AS
BEGIN
    UPDATE Projects 
    SET IsActive = 0, UpdatedDate = GETDATE()
    WHERE ProjectId = @ProjectId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteSkill]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Delete Skill (Soft Delete)
CREATE   PROCEDURE [dbo].[sp_DeleteSkill]
    @SkillId INT
AS
BEGIN
    UPDATE Skills 
    SET IsActive = 0, UpdatedDate = GETDATE()
    WHERE SkillId = @SkillId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetActiveChatSessions]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Active Chat Sessions
CREATE   PROCEDURE [dbo].[sp_GetActiveChatSessions]
    @Minutes INT = 30
AS
BEGIN
    SELECT DISTINCT VisitorId, VisitorName, VisitorEmail,
           MAX(CreatedDate) AS LastMessageTime,
           COUNT(*) AS MessageCount,
           SUM(CASE WHEN IsRead = 0 AND IsFromAdmin = 0 THEN 1 ELSE 0 END) AS UnreadCount
    FROM ChatMessages
    WHERE IsActive = 1 
      AND CreatedDate >= DATEADD(minute, -@Minutes, GETDATE())
    GROUP BY VisitorId, VisitorName, VisitorEmail
    ORDER BY LastMessageTime DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllAdmins]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get All Admins
CREATE   PROCEDURE [dbo].[sp_GetAllAdmins]
AS
BEGIN
    SELECT AdminId, Username, Email, IsActive, CreatedDate, UpdatedDate 
    FROM Admin 
    WHERE IsActive = 1 
    ORDER BY CreatedDate DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllBlogCategories]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================== BLOG CATEGORIES STORED PROCEDURES ====================

-- Get All Blog Categories
CREATE   PROCEDURE [dbo].[sp_GetAllBlogCategories]
AS
BEGIN
    SELECT * FROM BlogCategories 
    WHERE IsActive = 1 
    ORDER BY DisplayOrder, CategoryName;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllBlogPosts]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================== BLOG POSTS STORED PROCEDURES ====================

-- Get All Blog Posts
CREATE   PROCEDURE [dbo].[sp_GetAllBlogPosts]
AS
BEGIN
    SELECT p.*, c.CategoryName, c.Slug AS CategorySlug,
           (SELECT COUNT(*) FROM BlogImages WHERE PostId = p.PostId AND IsActive = 1) AS ImageCount,
           (SELECT COUNT(*) FROM BlogVideos WHERE PostId = p.PostId AND IsActive = 1) AS VideoCount,
           (SELECT COUNT(*) FROM BlogComments WHERE PostId = p.PostId AND IsActive = 1 AND IsApproved = 1) AS ApprovedCommentCount
    FROM BlogPosts p
    LEFT JOIN BlogCategories c ON p.CategoryId = c.CategoryId
    WHERE p.IsActive = 1
    ORDER BY p.IsFeatured DESC, p.PublishedDate DESC, p.CreatedDate DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllEducation]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================== EDUCATION STORED PROCEDURES ====================

-- Get All Education
CREATE   PROCEDURE [dbo].[sp_GetAllEducation]
AS
BEGIN
    SELECT * FROM Education 
    WHERE IsActive = 1 
    ORDER BY Year DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllExperience]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================== EXPERIENCE STORED PROCEDURES ====================

-- Get All Experience (Ordered by Date)
CREATE   PROCEDURE [dbo].[sp_GetAllExperience]
AS
BEGIN
    SELECT * FROM Experience 
    WHERE IsActive = 1 
    ORDER BY StartDate DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllGallery]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================== GALLERY STORED PROCEDURES ====================

-- Get All Gallery Items
CREATE   PROCEDURE [dbo].[sp_GetAllGallery]
    @MediaType NVARCHAR(50) = NULL
AS
BEGIN
    IF @MediaType IS NULL
        SELECT * FROM Gallery WHERE IsActive = 1 ORDER BY DisplayOrder, CreatedDate DESC;
    ELSE
        SELECT * FROM Gallery WHERE IsActive = 1 AND MediaType = @MediaType ORDER BY DisplayOrder, CreatedDate DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllMessages]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get All Messages
CREATE   PROCEDURE [dbo].[sp_GetAllMessages]
AS
BEGIN
    SELECT * FROM ContactMessages 
    WHERE IsActive = 1 
    ORDER BY CreatedDate DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllProjects]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================== PROJECTS STORED PROCEDURES ====================

-- Get All Active Projects
CREATE   PROCEDURE [dbo].[sp_GetAllProjects]
AS
BEGIN
    SELECT * FROM Projects 
    WHERE IsActive = 1 
    ORDER BY ProjectId DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllResumeViews]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get All Resume Views
CREATE   PROCEDURE [dbo].[sp_GetAllResumeViews]
AS
BEGIN
    SELECT * FROM ResumeViews 
    WHERE IsActive = 1 
    ORDER BY ViewDate DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllSkills]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================== SKILLS STORED PROCEDURES ====================

-- Get All Active Skills
CREATE   PROCEDURE [dbo].[sp_GetAllSkills]
AS
BEGIN
    SELECT * FROM Skills 
    WHERE IsActive = 1 
    ORDER BY SkillName;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetBlogCategoryById]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Category By Id
CREATE   PROCEDURE [dbo].[sp_GetBlogCategoryById]
    @CategoryId INT
AS
BEGIN
    SELECT * FROM BlogCategories 
    WHERE CategoryId = @CategoryId AND IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetBlogCategoryBySlug]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Category By Slug
CREATE   PROCEDURE [dbo].[sp_GetBlogCategoryBySlug]
    @Slug NVARCHAR(100)
AS
BEGIN
    SELECT * FROM BlogCategories 
    WHERE Slug = @Slug AND IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetBlogImagesByPost]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Blog Images By Post
CREATE   PROCEDURE [dbo].[sp_GetBlogImagesByPost]
    @PostId INT
AS
BEGIN
    SELECT * FROM BlogImages 
    WHERE PostId = @PostId AND IsActive = 1 
    ORDER BY DisplayOrder, CreatedDate;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetBlogPostById]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Blog Post By Id (with images, videos, comments)
CREATE   PROCEDURE [dbo].[sp_GetBlogPostById]
    @PostId INT
AS
BEGIN
    -- Get post details
    SELECT p.*, c.CategoryName, c.Slug AS CategorySlug
    FROM BlogPosts p
    LEFT JOIN BlogCategories c ON p.CategoryId = c.CategoryId
    WHERE p.PostId = @PostId AND p.IsActive = 1;
    
    -- Get images
    SELECT * FROM BlogImages 
    WHERE PostId = @PostId AND IsActive = 1 
    ORDER BY DisplayOrder, CreatedDate;
    
    -- Get videos
    SELECT * FROM BlogVideos 
    WHERE PostId = @PostId AND IsActive = 1 
    ORDER BY DisplayOrder, CreatedDate;
    
    -- Get approved comments
    SELECT c.*, vt.Country, vt.City 
    FROM BlogComments c
    LEFT JOIN VisitorTracking vt ON c.VisitorId = vt.VisitorId
    WHERE c.PostId = @PostId AND c.IsActive = 1 AND c.IsApproved = 1
    ORDER BY c.CreatedDate DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetBlogPostBySlug]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Blog Post By Slug (with view count update)
CREATE   PROCEDURE [dbo].[sp_GetBlogPostBySlug]
    @Slug NVARCHAR(500)
AS
BEGIN
    DECLARE @PostId INT;
    
    SELECT @PostId = PostId FROM BlogPosts WHERE Slug = @Slug AND IsActive = 1;
    
    IF @PostId IS NOT NULL
    BEGIN
        -- Update view count
        UPDATE BlogPosts SET ViewCount = ViewCount + 1 WHERE PostId = @PostId;
        
        -- Get post details
        EXEC sp_GetBlogPostById @PostId;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCategoriesWithPostCount]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Categories with Post Count
CREATE   PROCEDURE [dbo].[sp_GetCategoriesWithPostCount]
AS
BEGIN
    SELECT c.*, COUNT(p.PostId) AS PostCount
    FROM BlogCategories c
    LEFT JOIN BlogPosts p ON c.CategoryId = p.CategoryId AND p.IsActive = 1 AND p.IsPublished = 1
    WHERE c.IsActive = 1
    GROUP BY c.CategoryId, c.CategoryName, c.Slug, c.Description, c.Icon, c.DisplayOrder, c.IsActive, c.CreatedDate, c.UpdatedDate
    ORDER BY c.DisplayOrder, c.CategoryName;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetChatHistory]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Chat History
CREATE   PROCEDURE [dbo].[sp_GetChatHistory]
    @VisitorId INT
AS
BEGIN
    SELECT * FROM ChatMessages 
    WHERE VisitorId = @VisitorId AND IsActive = 1
    ORDER BY CreatedDate ASC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCommentsByPost]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Comments By Post
CREATE   PROCEDURE [dbo].[sp_GetCommentsByPost]
    @PostId INT
AS
BEGIN
    SELECT c.*, vt.Country, vt.City 
    FROM BlogComments c
    LEFT JOIN VisitorTracking vt ON c.VisitorId = vt.VisitorId
    WHERE c.PostId = @PostId AND c.IsActive = 1 AND c.IsApproved = 1
    ORDER BY c.CreatedDate DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCurrentExperience]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Current Experience
CREATE   PROCEDURE [dbo].[sp_GetCurrentExperience]
AS
BEGIN
    SELECT * FROM Experience 
    WHERE IsActive = 1 AND EndDate IS NULL 
    ORDER BY StartDate DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetEducationById]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Education By Id
CREATE   PROCEDURE [dbo].[sp_GetEducationById]
    @EducationId INT
AS
BEGIN
    SELECT * FROM Education 
    WHERE EducationId = @EducationId AND IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetEnhancedDashboardStats]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================== DASHBOARD STATISTICS STORED PROCEDURES ====================

-- Get Enhanced Dashboard Stats
CREATE   PROCEDURE [dbo].[sp_GetEnhancedDashboardStats]
AS
BEGIN
    -- Total Skills
    SELECT COUNT(*) AS TotalSkills FROM Skills WHERE IsActive = 1;
    
    -- Total Projects
    SELECT COUNT(*) AS TotalProjects FROM Projects WHERE IsActive = 1;
    
    -- Total Experience
    SELECT COUNT(*) AS TotalExperience FROM Experience WHERE IsActive = 1;
    
    -- Total Education
    SELECT COUNT(*) AS TotalEducation FROM Education WHERE IsActive = 1;
    
    -- Total Messages
    SELECT COUNT(*) AS TotalMessages FROM ContactMessages WHERE IsActive = 1;
    
    -- Unread Messages
    SELECT COUNT(*) AS UnreadMessages FROM ContactMessages WHERE IsActive = 1 AND IsRead = 0;
    
    -- Total Resume Views
    SELECT COUNT(*) AS TotalResumeViews FROM ResumeViews WHERE IsActive = 1;
    
    -- Total Downloads
    SELECT COUNT(*) AS TotalDownloads FROM ResumeViews WHERE IsDownloaded = 1;
    
    -- Total Blog Posts
    SELECT COUNT(*) AS TotalBlogPosts FROM BlogPosts WHERE IsActive = 1;
    
    -- Total Gallery Items
    SELECT COUNT(*) AS TotalGallery FROM Gallery WHERE IsActive = 1;
    
    -- Total Blog Comments
    SELECT COUNT(*) AS TotalComments FROM BlogComments WHERE IsActive = 1 AND IsApproved = 1;
    
    -- Pending Comments
    SELECT COUNT(*) AS PendingComments FROM BlogComments WHERE IsActive = 1 AND IsApproved = 0;
    
    -- Unique Visitors Today
    SELECT COUNT(DISTINCT VisitorId) AS TodayVisitors 
    FROM PageVisits 
    WHERE CAST(VisitTime AS DATE) = CAST(GETDATE() AS DATE);
    
    -- Active Chats (last 30 minutes)
    SELECT COUNT(DISTINCT VisitorId) AS ActiveChats 
    FROM ChatMessages 
    WHERE CreatedDate >= DATEADD(minute, -30, GETDATE()) AND IsActive = 1;
    
    -- Recent Messages (Last 5)
    SELECT TOP 5 * FROM ContactMessages 
    WHERE IsActive = 1 
    ORDER BY CreatedDate DESC;
    
    -- Recent Resume Views (Last 5)
    SELECT TOP 5 * FROM ResumeViews 
    WHERE IsActive = 1 
    ORDER BY ViewDate DESC;
    
    -- Recent Visitors (Last 5)
    SELECT TOP 5 * FROM VisitorTracking 
    WHERE IsActive = 1 
    ORDER BY LastVisit DESC;
    
    -- Recent Blog Posts (Last 5)
    SELECT TOP 5 PostId, Title, CreatedDate, ViewCount, LikeCount 
    FROM BlogPosts WHERE IsActive = 1 
    ORDER BY CreatedDate DESC;
    
    -- Popular Posts (Top 5)
    SELECT TOP 5 PostId, Title, ViewCount, LikeCount 
    FROM BlogPosts WHERE IsActive = 1 
    ORDER BY ViewCount DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetExperienceById]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Experience By Id
CREATE   PROCEDURE [dbo].[sp_GetExperienceById]
    @ExperienceId INT
AS
BEGIN
    SELECT * FROM Experience 
    WHERE ExperienceId = @ExperienceId AND IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetFeaturedProjects]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Featured Projects
CREATE   PROCEDURE [dbo].[sp_GetFeaturedProjects]
    @Count INT = 3
AS
BEGIN
    SELECT TOP (@Count) * FROM Projects 
    WHERE IsActive = 1 
    ORDER BY ProjectId DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetMessageById]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Message By Id
CREATE   PROCEDURE [dbo].[sp_GetMessageById]
    @MessageId INT
AS
BEGIN
    SELECT * FROM ContactMessages 
    WHERE MessageId = @MessageId AND IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetPageVisitsByVisitor]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Page Visits by Visitor
CREATE   PROCEDURE [dbo].[sp_GetPageVisitsByVisitor]
    @VisitorId INT
AS
BEGIN
    SELECT * FROM PageVisits 
    WHERE VisitorId = @VisitorId AND IsActive = 1
    ORDER BY VisitTime DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetPendingComments]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Pending Comments
CREATE   PROCEDURE [dbo].[sp_GetPendingComments]
AS
BEGIN
    SELECT c.*, p.Title AS PostTitle 
    FROM BlogComments c
    JOIN BlogPosts p ON c.PostId = p.PostId
    WHERE c.IsApproved = 0 AND c.IsActive = 1
    ORDER BY c.CreatedDate DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetPopularPosts]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Popular Posts
CREATE   PROCEDURE [dbo].[sp_GetPopularPosts]
    @Count INT = 5
AS
BEGIN
    SELECT TOP (@Count) * FROM BlogPosts 
    WHERE IsActive = 1 AND IsPublished = 1 
    ORDER BY ViewCount DESC, CreatedDate DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetProfile]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================== PROFILE STORED PROCEDURES ====================

-- Get Active Profile
CREATE   PROCEDURE [dbo].[sp_GetProfile]
AS
BEGIN
    SELECT TOP 1 * FROM Profile 
    WHERE IsActive = 1 
    ORDER BY ProfileId DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetProjectById]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Project By Id
CREATE   PROCEDURE [dbo].[sp_GetProjectById]
    @ProjectId INT
AS
BEGIN
    SELECT * FROM Projects 
    WHERE ProjectId = @ProjectId AND IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetPublishedBlogPosts]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Published Blog Posts (for public)
CREATE   PROCEDURE [dbo].[sp_GetPublishedBlogPosts]
    @CategoryId INT = NULL,
    @Tag NVARCHAR(100) = NULL,
    @Search NVARCHAR(500) = NULL
AS
BEGIN
    SELECT p.*, c.CategoryName, c.Slug AS CategorySlug,
           (SELECT COUNT(*) FROM BlogImages WHERE PostId = p.PostId AND IsActive = 1) AS ImageCount,
           (SELECT COUNT(*) FROM BlogComments WHERE PostId = p.PostId AND IsActive = 1 AND IsApproved = 1) AS CommentCount
    FROM BlogPosts p
    LEFT JOIN BlogCategories c ON p.CategoryId = c.CategoryId
    WHERE p.IsActive = 1 AND p.IsPublished = 1 
      AND (@CategoryId IS NULL OR p.CategoryId = @CategoryId)
      AND (@Tag IS NULL OR p.Tags LIKE '%' + @Tag + '%')
      AND (@Search IS NULL OR p.Title LIKE '%' + @Search + '%' OR p.Content LIKE '%' + @Search + '%' OR p.Excerpt LIKE '%' + @Search + '%')
    ORDER BY p.IsFeatured DESC, p.PublishedDate DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetRecentVisitors]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Recent Visitors
CREATE   PROCEDURE [dbo].[sp_GetRecentVisitors]
    @Days INT = 7
AS
BEGIN
    SELECT TOP 50 * FROM VisitorTracking 
    WHERE IsActive = 1 AND LastVisit >= DATEADD(day, -@Days, GETDATE())
    ORDER BY LastVisit DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetRelatedPosts]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Related Posts
CREATE   PROCEDURE [dbo].[sp_GetRelatedPosts]
    @PostId INT,
    @CategoryId INT,
    @Count INT = 3
AS
BEGIN
    SELECT TOP (@Count) * FROM BlogPosts 
    WHERE PostId != @PostId AND CategoryId = @CategoryId 
      AND IsActive = 1 AND IsPublished = 1
    ORDER BY CreatedDate DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetResumeCompleteData]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Resume Complete Data (for Crystal Reports)
CREATE   PROCEDURE [dbo].[sp_GetResumeCompleteData]
AS
BEGIN
    -- Get Profile Data
    SELECT * FROM Profile WHERE IsActive = 1;
    
    -- Get Skills Data
    SELECT * FROM Skills WHERE IsActive = 1 ORDER BY SkillName;
    
    -- Get Experience Data
    SELECT * FROM Experience WHERE IsActive = 1 ORDER BY StartDate DESC;
    
    -- Get Education Data
    SELECT * FROM Education WHERE IsActive = 1 ORDER BY Year DESC;
    
    -- Get Projects Data
    SELECT * FROM Projects WHERE IsActive = 1 ORDER BY ProjectId DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetResumeStats]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Resume Views Statistics
CREATE   PROCEDURE [dbo].[sp_GetResumeStats]
AS
BEGIN
    SELECT 
        COUNT(*) AS TotalViews,
        SUM(CASE WHEN IsDownloaded = 1 THEN 1 ELSE 0 END) AS TotalDownloads,
        COUNT(DISTINCT CompanyName) AS UniqueCompanies,
        COUNT(DISTINCT Country) AS UniqueCountries
    FROM ResumeViews 
    WHERE IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetResumeViewsByCompany]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Resume Views by Company
CREATE   PROCEDURE [dbo].[sp_GetResumeViewsByCompany]
    @CompanyName NVARCHAR(200)
AS
BEGIN
    SELECT * FROM ResumeViews 
    WHERE IsActive = 1 AND CompanyName LIKE '%' + @CompanyName + '%'
    ORDER BY ViewDate DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetSkillById]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Skill By Id
CREATE   PROCEDURE [dbo].[sp_GetSkillById]
    @SkillId INT
AS
BEGIN
    SELECT * FROM Skills 
    WHERE SkillId = @SkillId AND IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetSkillsSummary]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Skills Summary (for Dashboard)
CREATE   PROCEDURE [dbo].[sp_GetSkillsSummary]
AS
BEGIN
    SELECT 
        COUNT(*) AS TotalSkills,
        AVG(Percentage) AS AveragePercentage,
        MAX(Percentage) AS HighestSkill,
        MIN(Percentage) AS LowestSkill
    FROM Skills 
    WHERE IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetUnreadChatMessages]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Unread Chat Messages
CREATE   PROCEDURE [dbo].[sp_GetUnreadChatMessages]
AS
BEGIN
    SELECT cm.*, vt.Country, vt.City, vt.Organization 
    FROM ChatMessages cm
    LEFT JOIN VisitorTracking vt ON cm.VisitorId = vt.VisitorId
    WHERE cm.IsRead = 0 AND cm.IsActive = 1
    ORDER BY cm.CreatedDate DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetUnreadMessages]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Unread Messages
CREATE   PROCEDURE [dbo].[sp_GetUnreadMessages]
AS
BEGIN
    SELECT * FROM ContactMessages 
    WHERE IsActive = 1 AND IsRead = 0
    ORDER BY CreatedDate DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetVisitorById]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get Visitor Details
CREATE   PROCEDURE [dbo].[sp_GetVisitorById]
    @VisitorId INT
AS
BEGIN
    SELECT * FROM VisitorTracking 
    WHERE VisitorId = @VisitorId AND IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_IncrementGalleryDownload]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Increment Download Count
CREATE   PROCEDURE [dbo].[sp_IncrementGalleryDownload]
    @GalleryId INT
AS
BEGIN
    UPDATE Gallery SET DownloadCount = DownloadCount + 1 WHERE GalleryId = @GalleryId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_IncrementGalleryView]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Increment View Count
CREATE   PROCEDURE [dbo].[sp_IncrementGalleryView]
    @GalleryId INT
AS
BEGIN
    UPDATE Gallery SET ViewCount = ViewCount + 1 WHERE GalleryId = @GalleryId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_MarkChatAsRead]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Mark Chat as Read
CREATE   PROCEDURE [dbo].[sp_MarkChatAsRead]
    @ChatId INT
AS
BEGIN
    UPDATE ChatMessages 
    SET IsRead = 1, ReadDate = GETDATE()
    WHERE ChatId = @ChatId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_MarkMessageAsRead]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Mark Message as Read
CREATE   PROCEDURE [dbo].[sp_MarkMessageAsRead]
    @MessageId INT
AS
BEGIN
    UPDATE ContactMessages 
    SET IsRead = 1, UpdatedDate = GETDATE()
    WHERE MessageId = @MessageId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ReplyToMessage]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Reply to Message
CREATE   PROCEDURE [dbo].[sp_ReplyToMessage]
    @MessageId INT,
    @ReplyMessage NVARCHAR(MAX)
AS
BEGIN
    UPDATE ContactMessages 
    SET IsReplied = 1, 
        ReplyMessage = @ReplyMessage, 
        RepliedDate = GETDATE(),
        UpdatedDate = GETDATE()
    WHERE MessageId = @MessageId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_SaveChatMessage]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================== CHAT MESSAGES STORED PROCEDURES ====================

-- Save Chat Message
CREATE   PROCEDURE [dbo].[sp_SaveChatMessage]
    @VisitorId INT,
    @VisitorName NVARCHAR(100),
    @VisitorEmail NVARCHAR(100),
    @Message NVARCHAR(MAX),
    @IsFromAdmin BIT,
    @IPAddress NVARCHAR(50)
AS
BEGIN
    INSERT INTO ChatMessages (
        VisitorId, VisitorName, VisitorEmail, Message, 
        IsFromAdmin, IsRead, IPAddress, IsActive, CreatedDate
    )
    VALUES (
        @VisitorId, @VisitorName, @VisitorEmail, @Message,
        @IsFromAdmin, 0, @IPAddress, 1, GETDATE()
    );
    
    SELECT SCOPE_IDENTITY() AS ChatId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_SaveContactMessage]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================== CONTACT MESSAGES STORED PROCEDURES ====================

-- Save Contact Message
CREATE   PROCEDURE [dbo].[sp_SaveContactMessage]
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @Subject NVARCHAR(200),
    @Message NVARCHAR(MAX),
    @IPAddress NVARCHAR(50),
    @UserAgent NVARCHAR(500)
AS
BEGIN
    INSERT INTO ContactMessages (Name, Email, Subject, Message, IPAddress, UserAgent, IsActive, CreatedDate, UpdatedDate)
    VALUES (@Name, @Email, @Subject, @Message, @IPAddress, @UserAgent, 1, GETDATE(), GETDATE());
    
    SELECT SCOPE_IDENTITY() AS MessageId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_SetBlogCoverImage]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Set Cover Image
CREATE   PROCEDURE [dbo].[sp_SetBlogCoverImage]
    @ImageId INT,
    @PostId INT
AS
BEGIN
    UPDATE BlogImages SET IsCover = 0 WHERE PostId = @PostId;
    UPDATE BlogImages SET IsCover = 1 WHERE ImageId = @ImageId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ToggleBlogLike]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================== BLOG LIKES STORED PROCEDURES ====================

-- Toggle Blog Like
CREATE   PROCEDURE [dbo].[sp_ToggleBlogLike]
    @PostId INT,
    @VisitorId INT,
    @IPAddress NVARCHAR(50)
AS
BEGIN
    IF EXISTS(SELECT 1 FROM BlogLikes WHERE PostId = @PostId AND VisitorId = @VisitorId)
    BEGIN
        DELETE FROM BlogLikes WHERE PostId = @PostId AND VisitorId = @VisitorId;
        UPDATE BlogPosts SET LikeCount = LikeCount - 1 WHERE PostId = @PostId;
        SELECT 'unliked' AS Status;
    END
    ELSE
    BEGIN
        INSERT INTO BlogLikes (PostId, VisitorId, IPAddress, CreatedDate)
        VALUES (@PostId, @VisitorId, @IPAddress, GETDATE());
        UPDATE BlogPosts SET LikeCount = LikeCount + 1 WHERE PostId = @PostId;
        SELECT 'liked' AS Status;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_TrackResumeView]    Script Date: 08-03-2026 21:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================== RESUME VIEWS STORED PROCEDURES ====================

-- Track Resume View
CREATE   PROCEDURE [dbo].[sp_TrackResumeView]
    @VisitorName NVARCHAR(100),
    @VisitorEmail NVARCHAR(100),
    @CompanyName NVARCHAR(200),
    @Designation NVARCHAR(100),
    @IPAddress NVARCHAR(50),
    @UserAgent NVARCHAR(500),
    @Country NVARCHAR(100),
    @City NVARCHAR(100),
    @IsDownloaded BIT
AS
BEGIN
    INSERT INTO ResumeViews (
        VisitorName, VisitorEmail, CompanyName, Designation, 
        ViewDate, IPAddress, UserAgent, Country, City, 
        IsDownloaded, DownloadDate, IsActive, CreatedDate
    )
    VALUES (
        @VisitorName, @VisitorEmail, @CompanyName, @Designation,
        GETDATE(), @IPAddress, @UserAgent, @Country, @City,
        @IsDownloaded, CASE WHEN @IsDownloaded = 1 THEN GETDATE() ELSE NULL END,
        1, GETDATE()
    );
    
    SELECT SCOPE_IDENTITY() AS ViewId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_TrackVisitor]    Script Date: 08-03-2026 21:55:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================== VISITOR TRACKING STORED PROCEDURES ====================

-- Track or Update Visitor
CREATE   PROCEDURE [dbo].[sp_TrackVisitor]
    @SessionId NVARCHAR(100),
    @IPAddress NVARCHAR(50),
    @UserAgent NVARCHAR(500),
    @Country NVARCHAR(100),
    @City NVARCHAR(100),
    @Region NVARCHAR(100),
    @Latitude DECIMAL(10,8),
    @Longitude DECIMAL(11,8),
    @ISP NVARCHAR(200),
    @Organization NVARCHAR(200),
    @Timezone NVARCHAR(100),
    @PageUrl NVARCHAR(500),
    @PageTitle NVARCHAR(200)
AS
BEGIN
    DECLARE @VisitorId INT;
    DECLARE @PagesVisited NVARCHAR(MAX);
    
    -- Check if visitor exists
    SELECT @VisitorId = VisitorId, @PagesVisited = PagesVisited 
    FROM VisitorTracking 
    WHERE SessionId = @SessionId AND IsActive = 1;
    
    IF @VisitorId IS NULL
    BEGIN
        -- New visitor
        INSERT INTO VisitorTracking (
            SessionId, IPAddress, UserAgent, Country, City, Region,
            Latitude, Longitude, ISP, Organization, Timezone,
            FirstVisit, LastVisit, VisitCount, PagesVisited, IsActive, CreatedDate, UpdatedDate
        )
        VALUES (
            @SessionId, @IPAddress, @UserAgent, @Country, @City, @Region,
            @Latitude, @Longitude, @ISP, @Organization, @Timezone,
            GETDATE(), GETDATE(), 1, 
            ISNULL(@PageUrl, ''), 1, GETDATE(), GETDATE()
        );
        
        SET @VisitorId = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        -- Update existing visitor
        IF @PagesVisited IS NULL OR @PagesVisited = ''
            SET @PagesVisited = @PageUrl;
        ELSE IF @PageUrl IS NOT NULL AND CHARINDEX(@PageUrl, @PagesVisited) = 0
            SET @PagesVisited = @PagesVisited + ',' + @PageUrl;
        
        UPDATE VisitorTracking SET
            LastVisit = GETDATE(),
            VisitCount = VisitCount + 1,
            PagesVisited = @PagesVisited,
            UpdatedDate = GETDATE()
        WHERE VisitorId = @VisitorId;
    END
    
    -- Track page visit
    IF @PageUrl IS NOT NULL
    BEGIN
        INSERT INTO PageVisits (VisitorId, PageUrl, PageTitle, VisitTime, IsActive, CreatedDate)
        VALUES (@VisitorId, @PageUrl, @PageTitle, GETDATE(), 1, GETDATE());
    END
    
    SELECT @VisitorId AS VisitorId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateAdminPassword]    Script Date: 08-03-2026 21:55:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Update Admin Password
CREATE   PROCEDURE [dbo].[sp_UpdateAdminPassword]
    @AdminId INT,
    @NewPassword NVARCHAR(255)
AS
BEGIN
    UPDATE Admin 
    SET Password = @NewPassword, UpdatedDate = GETDATE()
    WHERE AdminId = @AdminId AND IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateBlogCategory]    Script Date: 08-03-2026 21:55:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Update Blog Category
CREATE   PROCEDURE [dbo].[sp_UpdateBlogCategory]
    @CategoryId INT,
    @CategoryName NVARCHAR(100),
    @Slug NVARCHAR(100),
    @Description NVARCHAR(500),
    @Icon NVARCHAR(100),
    @DisplayOrder INT
AS
BEGIN
    UPDATE BlogCategories SET
        CategoryName = @CategoryName,
        Slug = @Slug,
        Description = @Description,
        Icon = @Icon,
        DisplayOrder = @DisplayOrder,
        UpdatedDate = GETDATE()
    WHERE CategoryId = @CategoryId AND IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateBlogPost]    Script Date: 08-03-2026 21:55:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Update Blog Post
CREATE   PROCEDURE [dbo].[sp_UpdateBlogPost]
    @PostId INT,
    @CategoryId INT,
    @Title NVARCHAR(500),
    @Slug NVARCHAR(500),
    @Excerpt NVARCHAR(1000),
    @Content NVARCHAR(MAX),
    @FeaturedImage NVARCHAR(500),
    @VideoUrl NVARCHAR(500),
    @VideoEmbedCode NVARCHAR(MAX),
    @Tags NVARCHAR(500),
    @MetaTitle NVARCHAR(200),
    @MetaDescription NVARCHAR(500),
    @MetaKeywords NVARCHAR(500),
    @IsPublished BIT,
    @PublishedDate DATETIME,
    @IsFeatured BIT
AS
BEGIN
    UPDATE BlogPosts SET
        CategoryId = @CategoryId,
        Title = @Title,
        Slug = @Slug,
        Excerpt = @Excerpt,
        Content = @Content,
        FeaturedImage = @FeaturedImage,
        VideoUrl = @VideoUrl,
        VideoEmbedCode = @VideoEmbedCode,
        Tags = @Tags,
        MetaTitle = @MetaTitle,
        MetaDescription = @MetaDescription,
        MetaKeywords = @MetaKeywords,
        IsPublished = @IsPublished,
        PublishedDate = @PublishedDate,
        IsFeatured = @IsFeatured,
        UpdatedDate = GETDATE()
    WHERE PostId = @PostId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateEducation]    Script Date: 08-03-2026 21:55:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Update Education
CREATE   PROCEDURE [dbo].[sp_UpdateEducation]
    @EducationId INT,
    @Degree NVARCHAR(100),
    @Institute NVARCHAR(200),
    @Year INT,
    @Percentage DECIMAL(5,2)
AS
BEGIN
    UPDATE Education SET
        Degree = @Degree,
        Institute = @Institute,
        Year = @Year,
        Percentage = @Percentage,
        UpdatedDate = GETDATE()
    WHERE EducationId = @EducationId AND IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateExperience]    Script Date: 08-03-2026 21:55:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Update Experience
CREATE   PROCEDURE [dbo].[sp_UpdateExperience]
    @ExperienceId INT,
    @CompanyName NVARCHAR(200),
    @Role NVARCHAR(100),
    @StartDate DATE,
    @EndDate DATE,
    @Description NVARCHAR(MAX)
AS
BEGIN
    UPDATE Experience SET
        CompanyName = @CompanyName,
        Role = @Role,
        StartDate = @StartDate,
        EndDate = @EndDate,
        Description = @Description,
        UpdatedDate = GETDATE()
    WHERE ExperienceId = @ExperienceId AND IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateGalleryItem]    Script Date: 08-03-2026 21:55:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Update Gallery Item
CREATE   PROCEDURE [dbo].[sp_UpdateGalleryItem]
    @GalleryId INT,
    @Title NVARCHAR(500),
    @Description NVARCHAR(MAX),
    @MediaType NVARCHAR(50),
    @MediaPath NVARCHAR(500),
    @ThumbnailPath NVARCHAR(500),
    @VideoEmbedCode NVARCHAR(MAX),
    @Category NVARCHAR(100),
    @Tags NVARCHAR(500),
    @DisplayOrder INT,
    @IsFeatured BIT
AS
BEGIN
    UPDATE Gallery SET
        Title = @Title,
        Description = @Description,
        MediaType = @MediaType,
        MediaPath = @MediaPath,
        ThumbnailPath = @ThumbnailPath,
        VideoEmbedCode = @VideoEmbedCode,
        Category = @Category,
        Tags = @Tags,
        DisplayOrder = @DisplayOrder,
        IsFeatured = @IsFeatured,
        UpdatedDate = GETDATE()
    WHERE GalleryId = @GalleryId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateProfilePhoto]    Script Date: 08-03-2026 21:55:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Update Profile Photo
CREATE   PROCEDURE [dbo].[sp_UpdateProfilePhoto]
    @PhotoPath NVARCHAR(255)
AS
BEGIN
    UPDATE Profile 
    SET Photo = @PhotoPath, UpdatedDate = GETDATE()
    WHERE IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateProject]    Script Date: 08-03-2026 21:55:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Update Project
CREATE   PROCEDURE [dbo].[sp_UpdateProject]
    @ProjectId INT,
    @ProjectName NVARCHAR(200),
    @Description NVARCHAR(MAX),
    @ImagePath NVARCHAR(255),
    @GitHubLink NVARCHAR(500),
    @LiveLink NVARCHAR(500)
AS
BEGIN
    UPDATE Projects SET 
        ProjectName = @ProjectName,
        Description = @Description,
        ImagePath = @ImagePath,
        GitHubLink = @GitHubLink,
        LiveLink = @LiveLink,
        UpdatedDate = GETDATE()
    WHERE ProjectId = @ProjectId AND IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateResumePath]    Script Date: 08-03-2026 21:55:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Update Resume Path
CREATE   PROCEDURE [dbo].[sp_UpdateResumePath]
    @ResumePath NVARCHAR(255)
AS
BEGIN
    UPDATE Profile 
    SET ResumePath = @ResumePath, UpdatedDate = GETDATE()
    WHERE IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateSkill]    Script Date: 08-03-2026 21:55:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Update Skill
CREATE   PROCEDURE [dbo].[sp_UpdateSkill]
    @SkillId INT,
    @SkillName NVARCHAR(100),
    @Percentage INT
AS
BEGIN
    UPDATE Skills 
    SET SkillName = @SkillName, 
        Percentage = @Percentage,
        UpdatedDate = GETDATE()
    WHERE SkillId = @SkillId AND IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpsertProfile]    Script Date: 08-03-2026 21:55:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Insert or Update Profile
CREATE   PROCEDURE [dbo].[sp_UpsertProfile]
    @Name NVARCHAR(100),
    @Title NVARCHAR(100),
    @Description NVARCHAR(MAX),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(20),
    @Address NVARCHAR(200),
    @LinkedIn NVARCHAR(200),
    @GitHub NVARCHAR(200),
    @Photo NVARCHAR(255),
    @ResumePath NVARCHAR(255)
AS
BEGIN
    IF EXISTS(SELECT 1 FROM Profile WHERE IsActive = 1)
        UPDATE Profile SET 
            Name = @Name, 
            Title = @Title, 
            Description = @Description,
            Email = @Email, 
            Phone = @Phone, 
            Address = @Address,
            LinkedIn = @LinkedIn, 
            GitHub = @GitHub,
            Photo = @Photo, 
            ResumePath = @ResumePath,
            UpdatedDate = GETDATE()
        WHERE ProfileId = (SELECT TOP 1 ProfileId FROM Profile WHERE IsActive = 1);
    ELSE
        INSERT INTO Profile (Name, Title, Description, Email, Phone, Address, LinkedIn, GitHub, Photo, ResumePath, IsActive, CreatedDate, UpdatedDate)
        VALUES (@Name, @Title, @Description, @Email, @Phone, @Address, @LinkedIn, @GitHub, @Photo, @ResumePath, 1, GETDATE(), GETDATE());
END
GO

-- ==================== SEED DATA ====================
DELETE FROM Profile;
DELETE FROM Experience;
DELETE FROM Skills;
DELETE FROM Education;
DELETE FROM Projects;
GO

INSERT INTO Profile (Name, Title, Description, Email, Phone, Address, LinkedIn, GitHub, Photo, ResumePath, IsActive, CreatedDate, UpdatedDate)
VALUES (
    N'ALOK',
    N'ASP.NET Developer',
    N'Motivated ASP.NET Developer with 8 months of hands-on industry experience building scalable web applications using ASP.NET Core, RESTful APIs, and Angular. Passionate about clean architecture and contributing to high-impact software projects. Seeking a challenging role to grow technically and deliver production-grade solutions.',
    N'alokkanojiya96@gmail.com',
    N'8299078491',
    N'New Delhi',
    N'https://linkedin.com/in/alok8',
    N'https://github.com/Alokkannaujiya8',
    N'/uploads/photos/92235c45-473a-4393-94b7-45d95843dc08.png',
    N'/uploads/resumes/71bcbd35-8662-4e82-8c6e-ae925e2d88b7.pdf',
    1,
    GETDATE(),
    GETDATE()
);
GO

INSERT INTO Experience (CompanyName, Role, StartDate, EndDate, Description, IsActive, CreatedDate, UpdatedDate)
VALUES (
    N'Jogaz Info Pvt. Ltd',
    N'ASP.NET Developer',
    '2025-10-01',
    NULL,
    N'- Developing and maintaining web applications using ASP.NET Core Web API and ASP.NET Core MVC.
- Building and consuming RESTful APIs integrated with Angular front-end components.
- Designing and optimizing SQL Server database schemas, queries, and stored procedures.
- Implementing role-based authentication and authorization using ASP.NET Identity.
- Writing clean, maintainable code following Clean Architecture principles and SOLID design patterns.
- Participating in code reviews, debugging, and performance optimization of existing modules.',
    1,
    GETDATE(),
    GETDATE()
);
GO

INSERT INTO Projects (ProjectName, Description, ImagePath, GitHubLink, LiveLink, IsActive, CreatedDate, UpdatedDate)
VALUES 
(
    N'Human Resource Management System (HRMS)',
    N'Developed a full-stack HRMS with modules for employee onboarding, attendance tracking, leave management, and payroll reporting. Implemented role-based access control (RBAC) with Admin, HR Manager, and Employee roles. Built PDF/Excel reporting features for attendance and performance summaries. Integrated centralized logging for audit trails and system monitoring.',
    N'/uploads/projects/hrms.jpg',
    N'https://github.com/Alokkannaujiya8',
    N'',
    1,
    GETDATE(),
    GETDATE()
),
(
    N'Real-Time Chat Application',
    N'Built a real-time chat application with one-to-one and group messaging functionality. Implemented JWT Authentication, SignalR, and SQL Server for secure communication and message storage. Followed Clean Architecture and developed REST APIs for scalable and maintainable backend services.',
    N'/uploads/projects/chat.jpg',
    N'https://github.com/Alokkannaujiya8',
    N'',
    1,
    GETDATE(),
    GETDATE()
),
(
    N'Portfolio Management System',
    N'Developed a dynamic Portfolio Management System to showcase professional profile, skills, projects, and work experience. Designed and implemented custom SQL Server database with stored procedures, and integrated ADO.NET / DbHelper DAL for database operations. Features an Admin dashboard for managing projects, skills, education, blog posts, gallery, and visitor tracking. Includes real-time chat (SignalR), resume download tracking with visitor geolocation API integration, contact form with SMTP email services, and blog commenting system.',
    N'/uploads/projects/portfolio.jpg',
    N'https://github.com/Alokkannaujiya8',
    N'',
    1,
    GETDATE(),
    GETDATE()
);
GO

INSERT INTO Skills (SkillName, Percentage, IsActive, CreatedDate, UpdatedDate)
VALUES
(N'ASP.NET Core', 90, 1, GETDATE(), GETDATE()),
(N'ASP.NET Core Web API', 90, 1, GETDATE(), GETDATE()),
(N'ASP.NET Core MVC', 85, 1, GETDATE(), GETDATE()),
(N'ASP.NET Web Forms', 75, 1, GETDATE(), GETDATE()),
(N'RESTful API Development', 85, 1, GETDATE(), GETDATE()),
(N'Clean Architecture', 80, 1, GETDATE(), GETDATE()),
(N'Entity Framework Core', 85, 1, GETDATE(), GETDATE()),
(N'Angular', 80, 1, GETDATE(), GETDATE()),
(N'TypeScript', 80, 1, GETDATE(), GETDATE()),
(N'HTML5 & CSS3', 90, 1, GETDATE(), GETDATE()),
(N'SQL Server', 85, 1, GETDATE(), GETDATE()),
(N'MongoDB', 70, 1, GETDATE(), GETDATE()),
(N'Git & GitHub', 80, 1, GETDATE(), GETDATE()),
(N'Visual Studio / VS Code', 85, 1, GETDATE(), GETDATE()),
(N'Postman', 85, 1, GETDATE(), GETDATE());
GO

INSERT INTO Education (Degree, Institute, Year, Percentage, IsActive, CreatedDate, UpdatedDate)
VALUES
(N'Master of Computer Applications (MCA) - Computer Science and Engineering', N'HLM Group of Institutions, Ghaziabad | AKTU, Lucknow', 2027, NULL, 1, GETDATE(), GETDATE()),
(N'Bachelor of Computer Applications (BCA) - Computer Science and Engineering', N'Adhunik Institute of Education and Research, Ghaziabad | CCS University, Meerut', 2025, NULL, 1, GETDATE(), GETDATE());
GO

