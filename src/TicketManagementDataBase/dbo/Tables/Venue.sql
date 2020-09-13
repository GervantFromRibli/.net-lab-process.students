CREATE TABLE [dbo].[Venue] (
    [Id]          INT            NOT NULL,
    [Name]        NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (120) NOT NULL,
    [Address]     NVARCHAR (150) NOT NULL,
    [Phone]       NVARCHAR (15)  NULL,
    CONSTRAINT [PK_Venue] PRIMARY KEY CLUSTERED ([Id] ASC)
);

