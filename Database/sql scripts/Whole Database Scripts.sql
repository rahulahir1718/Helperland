USE [master]
GO
/****** Object:  Database [Helperland]    Script Date: 21-01-2022 11:57:39 ******/
CREATE DATABASE [Helperland]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Helperland', FILENAME = N'D:\SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Helperland.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Helperland_log', FILENAME = N'D:\SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Helperland_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Helperland] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Helperland].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Helperland] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Helperland] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Helperland] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Helperland] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Helperland] SET ARITHABORT OFF 
GO
ALTER DATABASE [Helperland] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Helperland] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Helperland] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Helperland] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Helperland] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Helperland] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Helperland] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Helperland] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Helperland] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Helperland] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Helperland] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Helperland] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Helperland] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Helperland] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Helperland] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Helperland] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Helperland] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Helperland] SET RECOVERY FULL 
GO
ALTER DATABASE [Helperland] SET  MULTI_USER 
GO
ALTER DATABASE [Helperland] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Helperland] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Helperland] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Helperland] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Helperland] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Helperland] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Helperland', N'ON'
GO
ALTER DATABASE [Helperland] SET QUERY_STORE = OFF
GO
USE [Helperland]
GO
/****** Object:  Table [dbo].[AddressDetails]    Script Date: 21-01-2022 11:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddressDetails](
	[AddressID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[StreetName] [varchar](80) NOT NULL,
	[HouseNo] [int] NOT NULL,
	[City] [varchar](30) NOT NULL,
	[PostalCode] [char](6) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlockedCustomers]    Script Date: 21-01-2022 11:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlockedCustomers](
	[ServiceProviderID] [int] NOT NULL,
	[CustomerID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CancelledServiceDetails]    Script Date: 21-01-2022 11:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CancelledServiceDetails](
	[ServiceID] [int] NOT NULL,
	[Reason] [varchar](70) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerLanguage]    Script Date: 21-01-2022 11:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerLanguage](
	[UserLanguage] [varchar](40) NOT NULL,
	[UserID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FavouriteSP]    Script Date: 21-01-2022 11:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FavouriteSP](
	[CustomerID] [int] NOT NULL,
	[FavouriteSPID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feedback]    Script Date: 21-01-2022 11:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedback](
	[CustomerID] [int] NOT NULL,
	[ServiceProviderID] [int] NOT NULL,
	[OnTimeArrival] [int] NOT NULL,
	[Friendly] [int] NOT NULL,
	[Quality] [int] NOT NULL,
	[Feedback] [varchar](100) NULL,
	[Rating] [decimal](2, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GetInTouchDetials]    Script Date: 21-01-2022 11:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GetInTouchDetials](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](40) NOT NULL,
	[LastName] [varchar](40) NOT NULL,
	[MobileNumber] [char](10) NULL,
	[Email] [varchar](60) NOT NULL,
	[UserSubject] [varchar](30) NOT NULL,
	[UserMessage] [varchar](200) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NewsLetterDetails]    Script Date: 21-01-2022 11:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NewsLetterDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](60) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 21-01-2022 11:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceDetails]    Script Date: 21-01-2022 11:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceDetails](
	[ServiceID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[ServiceDate] [date] NOT NULL,
	[ServiceTime] [time](7) NOT NULL,
	[ServiceDuration] [int] NOT NULL,
	[ExtraServices] [int] NULL,
	[Comments] [varchar](100) NULL,
	[HasPet] [bit] NOT NULL,
	[AcceptedBy] [int] NULL,
	[Payment] [decimal](4, 2) NOT NULL,
	[ServiceStatus] [varchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceProviderRatings]    Script Date: 21-01-2022 11:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceProviderRatings](
	[ServiceProviderID] [int] NOT NULL,
	[ratings] [decimal](2, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SPExtraDetails]    Script Date: 21-01-2022 11:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SPExtraDetails](
	[UserID] [int] NOT NULL,
	[Nationality] [varchar](30) NULL,
	[Gender] [varchar](20) NOT NULL,
	[AccountStatus] [bit] NOT NULL,
	[Avtar] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 21-01-2022 11:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](40) NOT NULL,
	[LastName] [varchar](40) NOT NULL,
	[Email] [varchar](60) NOT NULL,
	[MobileNumber] [char](10) NOT NULL,
	[UserPassword] [varchar](20) NOT NULL,
	[DOB] [date] NOT NULL,
	[RoleID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AddressDetails]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[BlockedCustomers]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[BlockedCustomers]  WITH CHECK ADD FOREIGN KEY([ServiceProviderID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[CancelledServiceDetails]  WITH CHECK ADD FOREIGN KEY([ServiceID])
REFERENCES [dbo].[ServiceDetails] ([ServiceID])
GO
ALTER TABLE [dbo].[CustomerLanguage]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[FavouriteSP]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[FavouriteSP]  WITH CHECK ADD FOREIGN KEY([FavouriteSPID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD FOREIGN KEY([ServiceProviderID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[ServiceDetails]  WITH CHECK ADD FOREIGN KEY([AcceptedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[ServiceDetails]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[ServiceProviderRatings]  WITH CHECK ADD FOREIGN KEY([ServiceProviderID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[SPExtraDetails]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
USE [master]
GO
ALTER DATABASE [Helperland] SET  READ_WRITE 
GO
