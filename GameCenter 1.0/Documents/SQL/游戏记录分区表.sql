DECLARE @maxValue INT,
    @secondMaxValue INT,
    @differ    INT,
    @fileGroupName VARCHAR(200),
    @fileNamePath    VARCHAR(200),
    @fileName   VARCHAR(200),
    @sql        NVARCHAR(1000)

SELECT @maxValue =CONVERT(INT,MAX(value)) FROM SYS.PARTITION_RANGE_VALUES PRV

SET @fileGroupName='GAMELOG' + CAST(@maxValue + 1 AS VARCHAR(4))
PRINT @fileGroupName
SET @sql='ALTER DATABASE [BW4_GameCenter] ADD FILEGROUP '+ @fileGroupName
PRINT @sql
EXEC(@sql)

SET @fileName = @fileGroupName + '-1'
SET @fileNamePath='D:\DataBase\BW4\'+ @fileName + '.NDF'
SET @sql='ALTER DATABASE [BW4_GameCenter] ADD FILE (NAME='''+@fileName+''',FILENAME=N'''+@fileNamePath+''') TO FILEGROUP '+ @fileGroupName
PRINT @sql
EXEC(@sql)
SET @fileName = @fileGroupName + '-2'
SET @fileNamePath='D:\DataBase\BW4\'+ @fileName + '.NDF'
SET @sql='ALTER DATABASE [BW4_GameCenter] ADD FILE (NAME='''+@fileName+''',FILENAME=N'''+@fileNamePath+''') TO FILEGROUP '+ @fileGroupName
PRINT @sql
EXEC(@sql)

--ALTER DATABASE [BW4_GameCenter] ADD FILEGROUP GAMELOG
--ALTER DATABASE [BW4_GameCenter] ADD FILE (NAME='GAMELOG-1',FILENAME=N'D:\DataBase\BW4\GAMELOG-1.NDF') TO FILEGROUP GAMELOG

SET @sql='ALTER PARTITION SCHEME [GameSchemeByType] NEXT USED '+  @fileGroupName
PRINT @sql
EXEC(@sql)

--分区函数
SET @sql='ALTER PARTITION FUNCTION GamePartitionByType() SPLIT RANGE (' + CAST(@maxValue+1 AS VARCHAR(4)) + ')'
PRINT @sql
EXEC(@sql)


-- 创建分区函数
CREATE PARTITION FUNCTION [GamePartitionByType](TINYINT) AS RANGE LEFT FOR VALUES (1, 2, 3)
-- 创建分区结构
CREATE PARTITION SCHEME [GameSchemeByType] 
AS PARTITION [GamePartitionByType] --分区函数
TO ([GAMELOG01],[GAMELOG02],[GAMELOG03],[GAMELOG])

-- 对已有表进行分区操作
USE [BW4_GameCenter]
GO
BEGIN TRANSACTION
ALTER TABLE [dbo].[game_Order] DROP CONSTRAINT [PK_game_Order] WITH ( ONLINE = OFF )
ALTER TABLE [dbo].[game_Order] ADD  CONSTRAINT [PK_game_Order] PRIMARY KEY NONCLUSTERED 
(
	[OrderID] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE CLUSTERED INDEX [ClusteredIndex_on_GameSchemeByType_637148757100600108] ON [dbo].[game_Order]
(
	[Game]
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [GameSchemeByType]([Game])
DROP INDEX [ClusteredIndex_on_GameSchemeByType_637148757100600108] ON [dbo].[game_Order]
COMMIT TRANSACTION


-- 检查分区
SELECT * FROM [dbo].game_Order WHERE $PARTITION.[GamePartitionByType](Game) = 1