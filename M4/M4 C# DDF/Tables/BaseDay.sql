CREATE TABLE [dbo].[BaseDay] (
    [DateTicks]  BIGINT        NOT NULL,
    [Date]       DATETIME      NOT NULL,
    [Symbol]     NVARCHAR (50) NOT NULL,
    [OpenPrice]  REAL          NULL,
    [HighPrice]  REAL          NULL,
    [LowPrice]   REAL          NULL,
    [ClosePrice] REAL          NULL,
    [VolumeF]    FLOAT (53)    NULL,
    [VolumeS]    BIGINT        NULL,
    [VolumeT]    BIGINT        NULL,
    [AdjustS]    REAL          NULL,
    [AdjustD]    REAL          NULL,
    PRIMARY KEY CLUSTERED ([DateTicks] ASC, [Symbol] ASC)
);


GO
CREATE NONCLUSTERED INDEX [SymbolIndex]
    ON [dbo].[BaseDay]([Symbol] ASC);


GO
CREATE NONCLUSTERED INDEX [DateIndex]
    ON [dbo].[BaseDay]([DateTicks] ASC);

