CREATE TABLE [dbo].[BusinessCard] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (100) NOT NULL,
    [Gender]          NVARCHAR (MAX) NOT NULL,
    [DateofBirth]     DATE           NOT NULL,
    [Email]           NVARCHAR (150) NOT NULL,
    [Phone]           NVARCHAR (20)  NOT NULL,
    [Photo]           NVARCHAR (MAX) NULL,
    [Address_Country] NVARCHAR (MAX) NULL,
    [Address_State]   NVARCHAR (MAX) NULL,
    [Address_ZipCode] NVARCHAR (MAX) NULL,
    [Address_City]    NVARCHAR (MAX) NULL,
    [Address_Street]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_BusinessCard] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_BusinessCard_Id]
    ON [dbo].[BusinessCard]([Id] ASC);


GO
