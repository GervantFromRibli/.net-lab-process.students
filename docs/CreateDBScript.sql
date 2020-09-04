use master;
 
GO

CREATE database TicketManagement;

GO

use TicketManagement;

GO

CREATE TABLE [Venue]
(
	[Id] int identity primary key,
	[Description] nvarchar(120) NOT NULL,
	[Address] nvarchar(200) NOT NULL,
	[Phone] nvarchar(30)
)

CREATE TABLE [Layout]
(
	[Id] int identity primary key,
	[VenueId] int NOT NULL,
	[Description] nvarchar(120) NOT NULL
)

ALTER TABLE dbo.Layout
ADD CONSTRAINT FK_Venue_Layout FOREIGN KEY (VenueId) REFERENCES dbo.Venue (Id)

CREATE TABLE [Area]
(
	[Id] int identity primary key,
	[LayoutId] int NOT NULL,
	[Description] nvarchar(200) NOT NULL,
	[CoordX] int NOT NULL,
	[CoordY] int NOT NULL,
)

ALTER TABLE dbo.Area
ADD CONSTRAINT FK_Layout_Area FOREIGN KEY (LayoutId) REFERENCES dbo.Layout (Id)

CREATE TABLE [Seat]
(
	[Id] int identity primary key,
	[AreaId] int NOT NULL,
	[Row] int NOT NULL,
	[Number] int NOT NULL,
)

ALTER TABLE dbo.Seat
ADD CONSTRAINT FK_Area_Seat FOREIGN KEY (AreaId) REFERENCES dbo.Area (Id)

CREATE TABLE [Event]
(
	[Id] int primary key identity,
	[Name] nvarchar(120) NOT NULL,
	[Description] nvarchar(400) NOT NULL,
	[LayoutId] int NOT NULL,
)

ALTER TABLE dbo.[Event]
ADD CONSTRAINT FK_Layout_Event FOREIGN KEY (LayoutId) REFERENCES dbo.Layout (Id)

CREATE TABLE [EventArea]
(
	[Id] int identity primary key,
	[EventId] int NOT NULL,
	[Description] nvarchar(200) NOT NULL,
	[CoordX] int NOT NULL,
	[CoordY] int NOT NULL,
	[Price] decimal NOT NULL
)

ALTER TABLE dbo.EventArea
ADD CONSTRAINT FK_Event_EventArea FOREIGN KEY ([EventId]) REFERENCES dbo.[Event] (Id)

CREATE TABLE [EventSeat]
(
	[Id] int identity primary key,
	[EventAreaId] int NOT NULL,
	[Row] int NOT NULL,
	[Number] int NOT NULL,
	[State] int NOT NULL
)

  
ALTER TABLE dbo.EventSeat
ADD CONSTRAINT FK_EventArea_EventSeat FOREIGN KEY ([EventAreaId]) REFERENCES dbo.EventArea (Id)