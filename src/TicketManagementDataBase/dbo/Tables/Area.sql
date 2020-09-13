CREATE TABLE [dbo].[Area] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [LayoutId]    INT            NOT NULL,
    [Description] NVARCHAR (200) NOT NULL,
    [CoordX]      INT            NOT NULL,
    [CoordY]      INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Area_Layout] FOREIGN KEY ([LayoutId]) REFERENCES [dbo].[Layout] ([Id]) ON DELETE CASCADE
);

