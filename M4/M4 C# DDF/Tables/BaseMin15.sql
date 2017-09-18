CREATE TABLE [dbo].[BaseMin15] (
    [DateTicks]  BIGINT        NOT NULL,
    [Date]       DATETIME      NOT NULL,
    [Symbol]     NVARCHAR (50) NOT NULL,
    [OpenPrice]  FLOAT (53)    NULL,
    [HighPrice]  FLOAT (53)    NULL,
    [LowPrice]   FLOAT (53)    NULL,
    [ClosePrice] FLOAT (53)    NULL,
    [VolumeF]    FLOAT (53)    NULL,
    [VolumeS]    BIGINT        NULL,
    [VolumeT]    BIGINT        NULL,
    [AdjustS]    FLOAT (53)    NULL,
    [AdjustD]    FLOAT (53)    NULL,
    PRIMARY KEY CLUSTERED ([DateTicks] ASC, [Symbol] ASC)
);

