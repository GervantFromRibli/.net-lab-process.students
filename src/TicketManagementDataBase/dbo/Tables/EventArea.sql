CREATE TABLE [dbo].[EventArea] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [EventId]     INT            NOT NULL,
    [Description] NVARCHAR (200) NOT NULL,
    [CoordX]      INT            NOT NULL,
    [CoordY]      INT            NOT NULL,
    [Price]       DECIMAL (18)   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EventArea_Event] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event] ([Id]) ON DELETE CASCADE
);

