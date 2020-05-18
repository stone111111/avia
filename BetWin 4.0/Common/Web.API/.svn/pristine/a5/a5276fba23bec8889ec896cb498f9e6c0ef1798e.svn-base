;WITH arg1 AS(
	SELECT * FROM sysobjects s WHERE s.name = '$(TABLE)'
)
SELECT 'TABLE' AS [Type],arg1.Name,ISNULL(f.value,'') as Description FROM arg1 LEFT JOIN sys.extended_properties f ON arg1.id = f.major_id and f.minor_id=0 

;WITH arg1 AS(
	SELECT * FROM syscolumns a WHERE EXISTS(SELECT 0 FROM sysobjects s WHERE s.name = '$(TABLE)' AND s.id = a.id)
),arg2 AS(
	SELECT a.colid, a.name,ISNULL(b.value,'') AS Description, a.xusertype,
		case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then 1 else 0 end AS [Identity],
		case when exists(SELECT 1 FROM sysobjects where xtype='PK' and parent_obj=a.id and name in ( 
	                 SELECT name FROM sysindexes WHERE indid in( SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid))) then 1 else 0 end AS [Key]
	FROM arg1 a LEFT JOIN sys.extended_properties b ON a.id = b.major_id AND a.colid = b.minor_id
)
SELECT 'COLUMN' AS [Type],ColID,arg2.Name,Description,[Identity],[Key],b.name AS [Type] FROM arg2 LEFT JOIN systypes b ON arg2.xusertype = b.xusertype
