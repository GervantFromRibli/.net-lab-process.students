USE [master]
GO
/****** Object:  Database [TicketManagement]    Script Date: 08.09.2020 23:45:38 ******/
CREATE DATABASE [TicketManagement]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TicketManagement', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\TicketManagement.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TicketManagement_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\TicketManagement_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [TicketManagement] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TicketManagement].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TicketManagement] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TicketManagement] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TicketManagement] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TicketManagement] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TicketManagement] SET ARITHABORT OFF 
GO
ALTER DATABASE [TicketManagement] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TicketManagement] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TicketManagement] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TicketManagement] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TicketManagement] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TicketManagement] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TicketManagement] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TicketManagement] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TicketManagement] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TicketManagement] SET  ENABLE_BROKER 
GO
ALTER DATABASE [TicketManagement] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TicketManagement] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TicketManagement] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TicketManagement] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TicketManagement] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TicketManagement] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TicketManagement] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TicketManagement] SET RECOVERY FULL 
GO
ALTER DATABASE [TicketManagement] SET  MULTI_USER 
GO
ALTER DATABASE [TicketManagement] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TicketManagement] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TicketManagement] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TicketManagement] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [TicketManagement] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'TicketManagement', N'ON'
GO
ALTER DATABASE [TicketManagement] SET QUERY_STORE = OFF
GO
USE [TicketManagement]
GO
/****** Object:  Table [dbo].[Area]    Script Date: 08.09.2020 23:45:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Area](
	[Id] [int] NOT NULL,
	[LayoutId] [int] NOT NULL,
	[Description] [nvarchar](200) NOT NULL,
	[CoordX] [int] NOT NULL,
	[CoordY] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Event]    Script Date: 08.09.2020 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](120) NOT NULL,
	[Description] [nvarchar](400) NOT NULL,
	[LayoutId] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventArea]    Script Date: 08.09.2020 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventArea](
	[Id] [int] NOT NULL,
	[EventId] [int] NOT NULL,
	[Description] [nvarchar](200) NOT NULL,
	[CoordX] [int] NOT NULL,
	[CoordY] [int] NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventSeat]    Script Date: 08.09.2020 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventSeat](
	[Id] [int] NOT NULL,
	[EventAreaId] [int] NOT NULL,
	[Row] [int] NOT NULL,
	[Number] [int] NOT NULL,
	[State] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Layout]    Script Date: 08.09.2020 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Layout](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[VenueId] [int] NOT NULL,
	[Description] [nvarchar](120) NOT NULL,
 CONSTRAINT [PK_Layout] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Seat]    Script Date: 08.09.2020 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Seat](
	[Id] [int] NOT NULL,
	[AreaId] [int] NOT NULL,
	[Row] [int] NOT NULL,
	[Number] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Venue]    Script Date: 08.09.2020 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Venue](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](120) NOT NULL,
	[Address] [nvarchar](150) NOT NULL,
	[Phone] [nvarchar](15) NULL,
 CONSTRAINT [PK_Venue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Area]  WITH CHECK ADD  CONSTRAINT [FK_Area_Layout] FOREIGN KEY([LayoutId])
REFERENCES [dbo].[Layout] ([Id])
GO
ALTER TABLE [dbo].[Area] CHECK CONSTRAINT [FK_Area_Layout]
GO
ALTER TABLE [dbo].[EventArea]  WITH CHECK ADD  CONSTRAINT [FK_EventArea_Event] FOREIGN KEY([EventId])
REFERENCES [dbo].[Event] ([Id])
GO
ALTER TABLE [dbo].[EventArea] CHECK CONSTRAINT [FK_EventArea_Event]
GO
ALTER TABLE [dbo].[EventSeat]  WITH CHECK ADD  CONSTRAINT [FK_EventArea_EventSeat] FOREIGN KEY([EventAreaId])
REFERENCES [dbo].[EventArea] ([Id])
GO
ALTER TABLE [dbo].[EventSeat] CHECK CONSTRAINT [FK_EventArea_EventSeat]
GO
ALTER TABLE [dbo].[Layout]  WITH CHECK ADD  CONSTRAINT [FK_Layout_Venue] FOREIGN KEY([VenueId])
REFERENCES [dbo].[Venue] ([Id])
GO
ALTER TABLE [dbo].[Layout] CHECK CONSTRAINT [FK_Layout_Venue]
GO
ALTER TABLE [dbo].[Seat]  WITH CHECK ADD  CONSTRAINT [FK_Area_Seat] FOREIGN KEY([AreaId])
REFERENCES [dbo].[Area] ([Id])
GO
ALTER TABLE [dbo].[Seat] CHECK CONSTRAINT [FK_Area_Seat]
GO
/****** Object:  StoredProcedure [dbo].[AddEvent]    Script Date: 08.09.2020 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddEvent]
    @Id INT,
    @Name NVARCHAR(120),
    @Descr NVARCHAR(400),
    @LayoutId INT
AS
INSERT INTO Event(Id, [Name], [Description], LayoutId) 
VALUES(@Id, @Name, @Descr, @LayoutId)
GO
/****** Object:  StoredProcedure [dbo].[DeleteEvent]    Script Date: 08.09.2020 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteEvent]
    @Id INT
AS
DELETE FROM Event WHERE Id = @Id
GO
/****** Object:  StoredProcedure [dbo].[UpdateEvent]    Script Date: 08.09.2020 23:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateEvent]
    @Id INT,
    @Name NVARCHAR(120),
    @Descr NVARCHAR(400),
    @LayoutId INT
AS
UPDATE Event SET [Name] = @Name, [Description] = @Descr, LayoutId = @LayoutId 
	WHERE Id = @Id 
GO
USE [master]
GO
ALTER DATABASE [TicketManagement] SET  READ_WRITE 
GO
