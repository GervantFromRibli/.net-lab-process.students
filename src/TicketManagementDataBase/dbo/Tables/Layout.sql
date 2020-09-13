CREATE TABLE [dbo].[Layout] (
    [Id]          INT            NOT NULL,
    [Name]        NVARCHAR (50)  NOT NULL,
    [VenueId]     INT            NOT NULL,
    [Description] NVARCHAR (120) NOT NULL,
    CONSTRAINT [PK_Layout] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Layout_Venue] FOREIGN KEY ([VenueId]) REFERENCES [dbo].[Venue] ([Id]) ON DELETE CASCADE
);

