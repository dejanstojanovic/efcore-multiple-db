USE [Database2]
GO
/****** Object:  Table [dbo].[Planets]    Script Date: 2020-12-11 23:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Planets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Table2] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Planets] ON 
GO
INSERT [dbo].[Planets] ([Id], [Name]) VALUES (1, N'Mercury')
GO
INSERT [dbo].[Planets] ([Id], [Name]) VALUES (2, N'Venus')
GO
INSERT [dbo].[Planets] ([Id], [Name]) VALUES (3, N'Earth')
GO
INSERT [dbo].[Planets] ([Id], [Name]) VALUES (4, N'Mars')
GO
INSERT [dbo].[Planets] ([Id], [Name]) VALUES (5, N'Jupiter')
GO
INSERT [dbo].[Planets] ([Id], [Name]) VALUES (6, N'Saturn')
GO
INSERT [dbo].[Planets] ([Id], [Name]) VALUES (7, N'Uranus')
GO
INSERT [dbo].[Planets] ([Id], [Name]) VALUES (8, N'Neptune')
GO
SET IDENTITY_INSERT [dbo].[Planets] OFF
GO
