CREATE TABLE [dbo].[Portfolios] (
    [pName]    NVARCHAR (50)  NOT NULL,
    [pIndex]   INT            NOT NULL,
    [pType]    INT            NOT NULL,
    [pSymbols] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([pName] ASC)
);

