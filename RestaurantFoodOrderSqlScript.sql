USE [master]
GO
/****** Object:  Database [RestaurantFoodDB]    Script Date: 08-Jun-21 8:08:56 PM ******/
CREATE DATABASE [RestaurantFoodDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RestaurantFoodDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\RestaurantFoodDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RestaurantFoodDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\RestaurantFoodDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [RestaurantFoodDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RestaurantFoodDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RestaurantFoodDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RestaurantFoodDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RestaurantFoodDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [RestaurantFoodDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RestaurantFoodDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [RestaurantFoodDB] SET  MULTI_USER 
GO
ALTER DATABASE [RestaurantFoodDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RestaurantFoodDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RestaurantFoodDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RestaurantFoodDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RestaurantFoodDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RestaurantFoodDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [RestaurantFoodDB] SET QUERY_STORE = OFF
GO
USE [RestaurantFoodDB]
GO
/****** Object:  Table [dbo].[FoodCategory]    Script Date: 08-Jun-21 8:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FoodCategory](
	[FoodCategoryid] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [varchar](50) NULL,
	[FoodMainID] [int] NOT NULL,
	[RestaurentID] [int] NOT NULL,
 CONSTRAINT [PK_FoodCategory] PRIMARY KEY CLUSTERED 
(
	[FoodCategoryid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MainFoodType]    Script Date: 08-Jun-21 8:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MainFoodType](
	[mainid] [int] IDENTITY(1,1) NOT NULL,
	[maincategory] [varchar](50) NULL,
 CONSTRAINT [PK_MainFoodType] PRIMARY KEY CLUSTERED 
(
	[mainid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MenuItems]    Script Date: 08-Jun-21 8:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuItems](
	[menuitemid] [int] IDENTITY(100,1) NOT NULL,
	[itemname] [nvarchar](max) NULL,
	[description] [nvarchar](max) NULL,
	[price] [int] NULL,
	[restaurentid] [int] NOT NULL,
	[FoodCategoryID] [int] NOT NULL,
	[MainFoodCategoryID] [int] NOT NULL,
	[itempic] [nvarchar](max) NULL,
 CONSTRAINT [PK_MenuItems] PRIMARY KEY CLUSTERED 
(
	[menuitemid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 08-Jun-21 8:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[orderid] [int] IDENTITY(2000,1) NOT NULL,
	[OrderByUser] [int] NOT NULL,
	[RestaurentID] [int] NOT NULL,
	[MenuItemID] [int] NOT NULL,
	[price] [int] NULL,
	[orderdate] [varchar](50) NULL,
	[session] [nvarchar](max) NULL,
	[paymentdone] [bit] NOT NULL,
	[finalorder] [bit] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[orderid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RestaurentRegistration]    Script Date: 08-Jun-21 8:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RestaurentRegistration](
	[RestaurentId] [int] IDENTITY(1,1) NOT NULL,
	[RestaurentEmail] [nvarchar](50) NULL,
	[emailvarified] [bit] NULL,
	[RestaurentCategory] [int] NOT NULL,
	[address] [nvarchar](max) NULL,
	[Mobile] [bigint] NULL,
	[State] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[Regdate] [varchar](50) NULL,
	[guid] [uniqueidentifier] NOT NULL,
	[password] [nvarchar](50) NOT NULL,
	[RestaurentName] [nvarchar](max) NOT NULL,
	[confirmpassword] [nvarchar](50) NULL,
 CONSTRAINT [PK_RestaurentRegistration] PRIMARY KEY CLUSTERED 
(
	[RestaurentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 08-Jun-21 8:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[userid] [int] IDENTITY(1000,1) NOT NULL,
	[email] [nvarchar](max) NULL,
	[password] [nvarchar](50) NULL,
	[userfoodpreference] [int] NULL,
	[emailvarified] [bit] NULL,
	[regdate] [varchar](50) NULL,
	[guid] [uniqueidentifier] NULL,
	[mobile] [bigint] NULL,
	[confirmpassword] [nvarchar](50) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[userid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_paymentdone]  DEFAULT ((0)) FOR [paymentdone]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_finalorder]  DEFAULT ((0)) FOR [finalorder]
GO
ALTER TABLE [dbo].[RestaurentRegistration] ADD  CONSTRAINT [DF_RestaurentRegistration_emailvarified]  DEFAULT ((0)) FOR [emailvarified]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_emailvarified]  DEFAULT ((0)) FOR [emailvarified]
GO
ALTER TABLE [dbo].[FoodCategory]  WITH CHECK ADD  CONSTRAINT [FK_FoodCategory_MainFoodType] FOREIGN KEY([FoodMainID])
REFERENCES [dbo].[MainFoodType] ([mainid])
GO
ALTER TABLE [dbo].[FoodCategory] CHECK CONSTRAINT [FK_FoodCategory_MainFoodType]
GO
ALTER TABLE [dbo].[FoodCategory]  WITH CHECK ADD  CONSTRAINT [FK_FoodCategory_RestaurentRegistration] FOREIGN KEY([RestaurentID])
REFERENCES [dbo].[RestaurentRegistration] ([RestaurentId])
GO
ALTER TABLE [dbo].[FoodCategory] CHECK CONSTRAINT [FK_FoodCategory_RestaurentRegistration]
GO
ALTER TABLE [dbo].[MenuItems]  WITH CHECK ADD  CONSTRAINT [FK_MenuItems_MainFoodType] FOREIGN KEY([MainFoodCategoryID])
REFERENCES [dbo].[MainFoodType] ([mainid])
GO
ALTER TABLE [dbo].[MenuItems] CHECK CONSTRAINT [FK_MenuItems_MainFoodType]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_MenuItems] FOREIGN KEY([MenuItemID])
REFERENCES [dbo].[MenuItems] ([menuitemid])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_MenuItems]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_RestaurentRegistration] FOREIGN KEY([RestaurentID])
REFERENCES [dbo].[RestaurentRegistration] ([RestaurentId])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_RestaurentRegistration]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_User] FOREIGN KEY([OrderByUser])
REFERENCES [dbo].[User] ([userid])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_User]
GO
ALTER TABLE [dbo].[RestaurentRegistration]  WITH CHECK ADD  CONSTRAINT [FK_RestaurentRegistration_MainFoodType] FOREIGN KEY([RestaurentCategory])
REFERENCES [dbo].[MainFoodType] ([mainid])
GO
ALTER TABLE [dbo].[RestaurentRegistration] CHECK CONSTRAINT [FK_RestaurentRegistration_MainFoodType]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_MainFoodType] FOREIGN KEY([userfoodpreference])
REFERENCES [dbo].[MainFoodType] ([mainid])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_MainFoodType]
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateOrder]    Script Date: 08-Jun-21 8:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_UpdateOrder] @CustomerID int , @SessionID nvarchar(50)
as
if exists(select orderid from Orders where OrderByUser=@CustomerID  and [session]=@SessionID)
BEGIN
update Orders set finalorder=1 where OrderByUser=@CustomerID  and [session]=@SessionID
END
GO
USE [master]
GO
ALTER DATABASE [RestaurantFoodDB] SET  READ_WRITE 
GO
