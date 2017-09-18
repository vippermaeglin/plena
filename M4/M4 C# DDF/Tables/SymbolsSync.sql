CREATE TABLE [dbo].[SymbolsSync] (
    [Code]      NVARCHAR (50) NOT NULL,
    [Name]      NVARCHAR (50) NULL,
    [Sector]    NVARCHAR (50) NULL,
    [SubSector] NVARCHAR (50) NULL,
    [Segment]   NVARCHAR (50) NULL,
    [Source]    NVARCHAR (50) NULL,
    [Type]      NVARCHAR (50) NULL,
    [Activity]  NVARCHAR (50) NULL,
    [Site]      NVARCHAR (50) NULL,
    [Status]    INT           NULL,
    [Priority]  INT           NULL,
    PRIMARY KEY CLUSTERED ([Code] ASC)
);


