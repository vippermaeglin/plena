CREATE VIEW [dbo].[PortfolioView]
	AS SELECT DateTicks, Date, Symbol, OpenPrice, HighPrice, LowPrice, ClosePrice, VolumeF, VolumeS, VolumeT, AdjustS, AdjustD 
		FROM (SELECT DateTicks, Date, Symbol, OpenPrice, HighPrice, LowPrice, ClosePrice, VolumeF, VolumeS, VolumeT, AdjustS, AdjustD,
			ROW_NUMBER() OVER (PARTITION BY Symbol ORDER BY DateTicks DESC) AS RowNumber
				FROM BaseDay) AS a
	WHERE a.RowNumber <=10;