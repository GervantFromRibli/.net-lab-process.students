CREATE TABLE [dbo].[Event] (
    [Id]          INT            NOT NULL,
    [Name]        NVARCHAR (120) NOT NULL,
    [Description] NVARCHAR (400) NOT NULL,
    [LayoutId]    INT            NOT NULL,
    [StartDate]   DATE           NOT NULL,
    [EndDate]     DATE           NOT NULL,
    CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Event_Layout] FOREIGN KEY ([LayoutId]) REFERENCES [dbo].[Layout] ([Id]) ON DELETE CASCADE
);

