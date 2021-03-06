USE [master]
GO
/****** Object:  Database [BetWin 4.0]    Script Date: 2020/2/29 4:59:55 ******/
CREATE DATABASE [BetWin 4.0]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BetWin 4.0', FILENAME = N'E:\wwwroot\BetWin 4.0\App_Data\BetWin 4.0.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BetWin 4.0_log', FILENAME = N'E:\wwwroot\BetWin 4.0\App_Data\BetWin 4.0_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [BetWin 4.0] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BetWin 4.0].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BetWin 4.0] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BetWin 4.0] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BetWin 4.0] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BetWin 4.0] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BetWin 4.0] SET ARITHABORT OFF 
GO
ALTER DATABASE [BetWin 4.0] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BetWin 4.0] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BetWin 4.0] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BetWin 4.0] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BetWin 4.0] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BetWin 4.0] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BetWin 4.0] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BetWin 4.0] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BetWin 4.0] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BetWin 4.0] SET  DISABLE_BROKER 
GO
ALTER DATABASE [BetWin 4.0] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BetWin 4.0] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BetWin 4.0] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BetWin 4.0] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BetWin 4.0] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BetWin 4.0] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BetWin 4.0] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BetWin 4.0] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BetWin 4.0] SET  MULTI_USER 
GO
ALTER DATABASE [BetWin 4.0] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BetWin 4.0] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BetWin 4.0] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BetWin 4.0] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BetWin 4.0] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BetWin 4.0] SET QUERY_STORE = OFF
GO
USE [BetWin 4.0]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [BetWin 4.0]
GO
/****** Object:  Table [dbo].[log_Error]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[log_Error](
	[ErrorID] [uniqueidentifier] NOT NULL,
	[SiteID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[IP] [varchar](20) NOT NULL,
	[IPAddress] [nvarchar](50) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_log_Error] PRIMARY KEY CLUSTERED 
(
	[ErrorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Site]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Site](
	[SiteID] [int] NOT NULL,
	[SiteName] [nvarchar](30) NOT NULL,
	[Currency] [tinyint] NOT NULL,
	[Language] [tinyint] NOT NULL,
	[Status] [tinyint] NOT NULL,
	[PCTemplate] [int] NOT NULL,
	[H5Template] [int] NOT NULL,
	[APPTemplate] [int] NOT NULL,
	[Setting] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Site] PRIMARY KEY CLUSTERED 
(
	[SiteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[site_Domain]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[site_Domain](
	[DomainID] [int] IDENTITY(1,1) NOT NULL,
	[SiteID] [int] NOT NULL,
	[Domain] [varchar](100) NOT NULL,
 CONSTRAINT [PK_site_Domain] PRIMARY KEY CLUSTERED 
(
	[DomainID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[site_SMS]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[site_SMS](
	[SMSID] [int] IDENTITY(1,1) NOT NULL,
	[GateID] [int] NOT NULL,
	[SiteID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Mobile] [varchar](20) NOT NULL,
	[Content] [nvarchar](200) NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[Status] [tinyint] NOT NULL,
	[ErrorMsg] [varchar](50) NOT NULL,
 CONSTRAINT [PK_site_SMS] PRIMARY KEY CLUSTERED 
(
	[SMSID] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_Admin]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_Admin](
	[AdminID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](20) NOT NULL,
	[Password] [char](40) NOT NULL,
	[NickName] [nvarchar](30) NOT NULL,
	[Face] [varchar](50) NOT NULL,
	[LoginAt] [smalldatetime] NOT NULL,
	[LoginIP] [varchar](20) NOT NULL,
	[Status] [tinyint] NOT NULL,
	[Permission] [varchar](max) NOT NULL,
	[SecretKey] [uniqueidentifier] NULL,
 CONSTRAINT [PK_sys_Admin] PRIMARY KEY CLUSTERED 
(
	[AdminID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_sys_Admin] UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_Setting]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_Setting](
	[Type] [tinyint] NOT NULL,
	[Value] [varchar](512) NOT NULL,
 CONSTRAINT [PK_sys_Setting] PRIMARY KEY CLUSTERED 
(
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_SMSSetting]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_SMSSetting](
	[GateID] [int] IDENTITY(1,1) NOT NULL,
	[GateName] [nvarchar](50) NOT NULL,
	[Type] [varchar](10) NOT NULL,
	[Setting] [nvarchar](max) NOT NULL,
	[Code] [varchar](256) NOT NULL,
	[IsOpen] [bit] NOT NULL,
 CONSTRAINT [PK_sys_SMSSetting] PRIMARY KEY CLUSTERED 
(
	[GateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(10000,1) NOT NULL,
	[SiteID] [int] NOT NULL,
	[UserName] [varchar](32) NOT NULL,
	[Password] [char](32) NOT NULL,
	[PayPassword] [char](32) NOT NULL,
	[CreateAt] [smalldatetime] NOT NULL,
	[RegIP] [varchar](20) NOT NULL,
	[RegDomain] [varchar](50) NOT NULL,
	[RegPlatform] [tinyint] NOT NULL,
	[LoginAt] [smalldatetime] NOT NULL,
	[LoginIP] [varchar](20) NOT NULL,
	[LoginDomain] [varchar](50) NOT NULL,
	[LoginPlatform] [tinyint] NOT NULL,
	[Currency] [tinyint] NOT NULL,
	[Money] [money] NOT NULL,
	[LockMoney] [money] NOT NULL,
	[GameMoney] [money] NOT NULL,
	[Language] [tinyint] NOT NULL,
	[AgentID] [int] NOT NULL,
	[InviteID] [int] NOT NULL,
	[Lock] [tinyint] NOT NULL,
	[Function] [tinyint] NOT NULL,
	[Type] [tinyint] NOT NULL,
	[IsTest] [bit] NOT NULL,
	[Mobile] [varchar](20) NOT NULL,
	[IsMobile] [bit] NOT NULL,
	[Email] [varchar](128) NOT NULL,
	[IsEmail] [bit] NOT NULL,
	[Face] [varchar](50) NOT NULL,
	[NickName] [nvarchar](30) NOT NULL,
	[GroupID] [int] NOT NULL,
	[Question] [tinyint] NOT NULL,
	[Answer] [char](32) NOT NULL,
	[LevelID] [int] NOT NULL,
	[AccountName] [nvarchar](30) NOT NULL,
	[Time] [timestamp] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[usr_Depth]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[usr_Depth](
	[DepthID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ChildID] [int] NOT NULL,
	[SiteID] [int] NOT NULL,
	[Depth] [int] NOT NULL,
 CONSTRAINT [PK_usr_Depth] PRIMARY KEY CLUSTERED 
(
	[DepthID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[usr_Invite]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[usr_Invite](
	[InviteID] [varchar](30) NOT NULL,
	[SiteID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[CreateAt] [smalldatetime] NOT NULL,
	[Member] [int] NOT NULL,
	[IsOpen] [bit] NOT NULL,
 CONSTRAINT [PK_usr_Invite] PRIMARY KEY CLUSTERED 
(
	[InviteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[usr_Log]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[usr_Log](
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[SiteID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Type] [tinyint] NOT NULL,
	[IP] [varchar](20) NOT NULL,
	[Platform] [tinyint] NOT NULL,
	[IMEI] [char](32) NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[Content] [nvarchar](200) NOT NULL,
	[AdminID] [int] NOT NULL,
 CONSTRAINT [PK_usr_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_Content]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_Content](
	[ModelID] [int] NOT NULL,
	[Language] [tinyint] NOT NULL,
	[Path] [varchar](50) NOT NULL,
	[Translate] [numeric](3, 2) NOT NULL,
 CONSTRAINT [PK_view_Content_1] PRIMARY KEY CLUSTERED 
(
	[ModelID] ASC,
	[Language] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_Model]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_Model](
	[ModelID] [int] IDENTITY(1,1) NOT NULL,
	[ViewID] [int] NOT NULL,
	[ModelName] [nvarchar](50) NOT NULL,
	[ModelDesc] [nvarchar](512) NOT NULL,
	[Preview] [varchar](50) NOT NULL,
	[Path] [varchar](50) NOT NULL,
	[Page] [nvarchar](max) NOT NULL,
	[Style] [nvarchar](max) NOT NULL,
	[Resources] [varchar](max) NOT NULL,
 CONSTRAINT [PK_view_Model] PRIMARY KEY CLUSTERED 
(
	[ModelID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_Setting]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_Setting](
	[ViewID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Platform] [tinyint] NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[Status] [tinyint] NOT NULL,
	[Sort] [smallint] NOT NULL,
 CONSTRAINT [PK_view_Setting] PRIMARY KEY CLUSTERED 
(
	[ViewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_view_Setting] UNIQUE NONCLUSTERED 
(
	[Platform] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_SiteConfig]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_SiteConfig](
	[ConfigID] [int] IDENTITY(1,1) NOT NULL,
	[TemplateID] [int] NOT NULL,
	[SiteID] [int] NOT NULL,
	[ViewID] [int] NOT NULL,
	[ModelID] [int] NOT NULL,
	[Setting] [varchar](max) NOT NULL,
 CONSTRAINT [PK_view_Config] PRIMARY KEY CLUSTERED 
(
	[ConfigID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_SiteTemplate]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_SiteTemplate](
	[TemplateID] [int] IDENTITY(1,1) NOT NULL,
	[SiteID] [int] NOT NULL,
	[Platform] [tinyint] NOT NULL,
	[TemplateName] [nvarchar](30) NOT NULL,
	[Domain] [varchar](1024) NOT NULL,
 CONSTRAINT [PK_view_SiteTemplate] PRIMARY KEY CLUSTERED 
(
	[TemplateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_Template]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_Template](
	[TemplateID] [int] IDENTITY(1,1) NOT NULL,
	[Platform] [tinyint] NOT NULL,
	[TemplateName] [nvarchar](30) NOT NULL,
	[Preview] [varchar](50) NOT NULL,
	[Sort] [smallint] NOT NULL,
 CONSTRAINT [PK_view_Template] PRIMARY KEY CLUSTERED 
(
	[TemplateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_TemplateConfig]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_TemplateConfig](
	[ConfigID] [int] IDENTITY(1,1) NOT NULL,
	[TemplateID] [int] NOT NULL,
	[ViewID] [int] NOT NULL,
	[ModelID] [int] NOT NULL,
	[Setting] [varchar](max) NOT NULL,
 CONSTRAINT [PK_view_TemplateConfig] PRIMARY KEY CLUSTERED 
(
	[ConfigID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_view_Config]    Script Date: 2020/2/29 4:59:56 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_view_Config] ON [dbo].[view_SiteConfig]
(
	[TemplateID] ASC,
	[ViewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[site_Domain]  WITH CHECK ADD  CONSTRAINT [FK_site_Domain_Site] FOREIGN KEY([SiteID])
REFERENCES [dbo].[Site] ([SiteID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[site_Domain] CHECK CONSTRAINT [FK_site_Domain_Site]
GO
ALTER TABLE [dbo].[site_SMS]  WITH CHECK ADD  CONSTRAINT [FK_site_SMS_sys_SMSSetting] FOREIGN KEY([GateID])
REFERENCES [dbo].[sys_SMSSetting] ([GateID])
GO
ALTER TABLE [dbo].[site_SMS] CHECK CONSTRAINT [FK_site_SMS_sys_SMSSetting]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Site] FOREIGN KEY([SiteID])
REFERENCES [dbo].[Site] ([SiteID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Site]
GO
ALTER TABLE [dbo].[view_Content]  WITH CHECK ADD  CONSTRAINT [FK_view_Content_view_Model] FOREIGN KEY([ModelID])
REFERENCES [dbo].[view_Model] ([ModelID])
GO
ALTER TABLE [dbo].[view_Content] CHECK CONSTRAINT [FK_view_Content_view_Model]
GO
ALTER TABLE [dbo].[view_Model]  WITH CHECK ADD  CONSTRAINT [FK_view_Model_view_Setting] FOREIGN KEY([ViewID])
REFERENCES [dbo].[view_Setting] ([ViewID])
GO
ALTER TABLE [dbo].[view_Model] CHECK CONSTRAINT [FK_view_Model_view_Setting]
GO
ALTER TABLE [dbo].[view_SiteConfig]  WITH CHECK ADD  CONSTRAINT [FK_view_SiteConfig_view_SiteTemplate] FOREIGN KEY([TemplateID])
REFERENCES [dbo].[view_SiteTemplate] ([TemplateID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[view_SiteConfig] CHECK CONSTRAINT [FK_view_SiteConfig_view_SiteTemplate]
GO
ALTER TABLE [dbo].[view_TemplateConfig]  WITH CHECK ADD  CONSTRAINT [FK_view_TemplateConfig_view_Template] FOREIGN KEY([TemplateID])
REFERENCES [dbo].[view_Template] ([TemplateID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[view_TemplateConfig] CHECK CONSTRAINT [FK_view_TemplateConfig_view_Template]
GO
/****** Object:  StoredProcedure [dbo].[usr_CreateDepth]    Script Date: 2020/2/29 4:59:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usr_CreateDepth]
@SiteID INT,	-- 所属商户
@UserID INT,	-- 上级ID
@ChildID INT	-- 下级ID
AS
WITH arg1 AS(
  SELECT UserID,@ChildID as ChildID,Depth + 1 as Depth FROM usr_Depth WHERE ChildID = @UserID
  UNION ALL
  SELECT @ChildID,@ChildID,0
)
INSERT INTO usr_Depth(UserID,ChildID,SiteID,Depth) SELECT UserID,ChildID,@SiteID,Depth FROM arg1

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发生错误的商户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'log_Error', @level2type=N'COLUMN',@level2name=N'SiteID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产生错误的用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'log_Error', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误发生时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'log_Error', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'log_Error', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'log_Error', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ErrorLog]错误日志' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'log_Error'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ID]商户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Site', @level2type=N'COLUMN',@level2name=N'SiteID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Name]商户名（内部识别）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Site', @level2type=N'COLUMN',@level2name=N'SiteName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=Currency]默认币种' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Site', @level2type=N'COLUMN',@level2name=N'Currency'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=Language]默认语种' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Site', @level2type=N'COLUMN',@level2name=N'Language'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=SiteStatus]商户状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Site', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'默认的PC端模板' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Site', @level2type=N'COLUMN',@level2name=N'PCTemplate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'默认的H5端模板' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Site', @level2type=N'COLUMN',@level2name=N'H5Template'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'默认的APP模板' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Site', @level2type=N'COLUMN',@level2name=N'APPTemplate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[SettingString]商户设置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Site', @level2type=N'COLUMN',@level2name=N'Setting'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ID]域名ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'site_Domain', @level2type=N'COLUMN',@level2name=N'DomainID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'域名（根域名）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'site_Domain', @level2type=N'COLUMN',@level2name=N'Domain'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[SiteDomain]商户的域名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'site_Domain'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ID]短信验证码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'site_SMS', @level2type=N'COLUMN',@level2name=N'SMSID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所使用的渠道' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'site_SMS', @level2type=N'COLUMN',@level2name=N'GateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送的用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'site_SMS', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机号码（包含区号）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'site_SMS', @level2type=N'COLUMN',@level2name=N'Mobile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'短信内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'site_SMS', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'site_SMS', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=SMSStatus]发送状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'site_SMS', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'site_SMS', @level2type=N'COLUMN',@level2name=N'ErrorMsg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[SiteSMS]短信发送记录' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'site_SMS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ID]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_Admin', @level2type=N'COLUMN',@level2name=N'AdminID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_Admin', @level2type=N'COLUMN',@level2name=N'Password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'昵称（用于对外发布信息需要展示的名字）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_Admin', @level2type=N'COLUMN',@level2name=N'NickName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自定义的头像' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_Admin', @level2type=N'COLUMN',@level2name=N'Face'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上次登录时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_Admin', @level2type=N'COLUMN',@level2name=N'LoginAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上次登录IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_Admin', @level2type=N'COLUMN',@level2name=N'LoginIP'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=AdminStatus]状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_Admin', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_Admin', @level2type=N'COLUMN',@level2name=N'Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_Admin', @level2type=N'COLUMN',@level2name=N'SecretKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[SystemAdmin]系统管理员' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_Admin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=SettingType]设置类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_Setting', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[SystemSetting]全局的参数设定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_Setting'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ID]短信通道编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_SMSSetting', @level2type=N'COLUMN',@level2name=N'GateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通道类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_SMSSetting', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_SMSSetting', @level2type=N'COLUMN',@level2name=N'Setting'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支持的地区，逗号隔开' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_SMSSetting', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否开放' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_SMSSetting', @level2type=N'COLUMN',@level2name=N'IsOpen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[SMSSetting]短信网关设定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_SMSSetting'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ID]用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属商户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'SiteID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名（只允许数字+字母）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码（MD5加密）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资金密码（SHA+MD5双重加密），6位数字格式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'PayPassword'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'RegIP'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册来源域名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'RegDomain'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=PlatformType]注册设备' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'RegPlatform'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最近一次登录的时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'LoginAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'LoginIP'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录域名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'LoginDomain'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=PlatformType]登录设备' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'LoginPlatform'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=Currency]会员币种（默认继承自上级代理的币种）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Currency'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'当前可用余额（非时时）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Money'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'当前锁定金额（非实时）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'LockMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'当前游戏账户总余额（非实时)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'GameMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=Language]默认语种（继承自上级代理的语种）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Language'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上级代理ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'AgentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邀请者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'InviteID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=LockType]当前会员被锁定的功能（位枚举）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Lock'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=FunctionType]当前会员可开放的功能' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Function'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=UserType]会员类型，股东、总代、代理、会员' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标记为测试账号（测试账号必须整条线都是测试账号，不可更改)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'IsTest'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Mobile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机是否通过了验证' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'IsMobile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'电子邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮箱是否通过验证' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'IsEmail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'头像' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Face'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'昵称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'NickName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属分组' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'GroupID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=QuestionType]安全问题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Question'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'安全问题答案' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Answer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所处的VIP等级（可以为0），对应 usr_Level 表的ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'LevelID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提现的银行卡姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'AccountName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=byte[]]更改的时间戳，唯一标记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[User]会员' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ID]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'usr_Depth', @level2type=N'COLUMN',@level2name=N'DepthID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'usr_Depth', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'usr_Depth', @level2type=N'COLUMN',@level2name=N'ChildID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属商户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'usr_Depth', @level2type=N'COLUMN',@level2name=N'SiteID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'当前关系的层数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'usr_Depth', @level2type=N'COLUMN',@level2name=N'Depth'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[UserDepth]用户的层级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'usr_Depth'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ID]邀请码，全局唯一' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'usr_Invite', @level2type=N'COLUMN',@level2name=N'InviteID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用该邀请码注册的用户数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'usr_Invite', @level2type=N'COLUMN',@level2name=N'Member'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否开启该邀请码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'usr_Invite', @level2type=N'COLUMN',@level2name=N'IsOpen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[UserInvite]邀请码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'usr_Invite'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ID]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'usr_Log', @level2type=N'COLUMN',@level2name=N'LogID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=LogType]操作类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'usr_Log', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=PlatformType]操作设备' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'usr_Log', @level2type=N'COLUMN',@level2name=N'Platform'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'设备编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'usr_Log', @level2type=N'COLUMN',@level2name=N'IMEI'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'usr_Log', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'usr_Log', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理员操作的会员信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'usr_Log', @level2type=N'COLUMN',@level2name=N'AdminID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[UserLog]用户操作日志' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'usr_Log'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所关联的模型编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Content', @level2type=N'COLUMN',@level2name=N'ModelID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=Language]所适配的语种' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Content', @level2type=N'COLUMN',@level2name=N'Language'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Content', @level2type=N'COLUMN',@level2name=N'Path'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'翻译进度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Content', @level2type=N'COLUMN',@level2name=N'Translate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ViewContent]视图模型的内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ID]视图模板编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Model', @level2type=N'COLUMN',@level2name=N'ModelID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属的视图' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Model', @level2type=N'COLUMN',@level2name=N'ViewID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Name]视图模型名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Model', @level2type=N'COLUMN',@level2name=N'ModelName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Description]视图模板的配置说明（来自于页面文件内，格式：<!-- 说明内容 -->）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Model', @level2type=N'COLUMN',@level2name=N'ModelDesc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预览图' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Model', @level2type=N'COLUMN',@level2name=N'Preview'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'视图模型的路径，在PC和H5环境中用于测试，在APP中用于定位视图资源的路径/名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Model', @level2type=N'COLUMN',@level2name=N'Path'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'页面文件内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Model', @level2type=N'COLUMN',@level2name=N'Page'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'样式文件内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Model', @level2type=N'COLUMN',@level2name=N'Style'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资源文件路径，格式：{"logo.png":"images/{MD5}.png"}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Model', @level2type=N'COLUMN',@level2name=N'Resources'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ViewModel]视图的模型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Model'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ID]视图的编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Setting', @level2type=N'COLUMN',@level2name=N'ViewID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'视图名字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Setting', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=PlatformSource]所属平台（PC、H5、APP）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Setting', @level2type=N'COLUMN',@level2name=N'Platform'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'视图所对应的类路径（完整）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Setting', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=ViewStatus]视图的状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Setting', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自定义的排序（从大到小）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Setting', @level2type=N'COLUMN',@level2name=N'Sort'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ViewSetting]系统的视图配置，从类库中读取' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Setting'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ID]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_SiteConfig', @level2type=N'COLUMN',@level2name=N'ConfigID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属的商户模板' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_SiteConfig', @level2type=N'COLUMN',@level2name=N'TemplateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属商户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_SiteConfig', @level2type=N'COLUMN',@level2name=N'SiteID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属的视图' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_SiteConfig', @level2type=N'COLUMN',@level2name=N'ViewID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所选择的视图模型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_SiteConfig', @level2type=N'COLUMN',@level2name=N'ModelID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商户的配置内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_SiteConfig', @level2type=N'COLUMN',@level2name=N'Setting'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ViewSiteConfig]商户的视图配置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_SiteConfig'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ID]配置编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_SiteTemplate', @level2type=N'COLUMN',@level2name=N'TemplateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'模板所属商户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_SiteTemplate', @level2type=N'COLUMN',@level2name=N'SiteID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=PlatformSource]模板所属平台' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_SiteTemplate', @level2type=N'COLUMN',@level2name=N'Platform'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Name]模板名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_SiteTemplate', @level2type=N'COLUMN',@level2name=N'TemplateName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所要绑定到的域名（支持通配符），多个域名用逗号隔开。' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_SiteTemplate', @level2type=N'COLUMN',@level2name=N'Domain'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ViewSiteTemplate]商户的模板配置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_SiteTemplate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ID]模板编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Template', @level2type=N'COLUMN',@level2name=N'TemplateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Type=PlatformType]适配的平台' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Template', @level2type=N'COLUMN',@level2name=N'Platform'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Name]模板名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Template', @level2type=N'COLUMN',@level2name=N'TemplateName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'模板预览图' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Template', @level2type=N'COLUMN',@level2name=N'Preview'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序（从大到小）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Template', @level2type=N'COLUMN',@level2name=N'Sort'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ViewTemplate]系统配置的模板，开站的时候选择' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_Template'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ID]配置编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_TemplateConfig', @level2type=N'COLUMN',@level2name=N'ConfigID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属的视图' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_TemplateConfig', @level2type=N'COLUMN',@level2name=N'ViewID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所选择的模板' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_TemplateConfig', @level2type=N'COLUMN',@level2name=N'ModelID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'该模板对于此视图的参数配置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_TemplateConfig', @level2type=N'COLUMN',@level2name=N'Setting'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ViewTemplateConfig]系统模板配置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'view_TemplateConfig'
GO
USE [master]
GO
ALTER DATABASE [BetWin 4.0] SET  READ_WRITE 
GO
