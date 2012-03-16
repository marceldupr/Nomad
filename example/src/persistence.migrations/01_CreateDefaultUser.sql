IF NOT EXISTS (SELECT loginname FROM master.dbo.syslogins WHERE name = '{1}')
BEGIN
    CREATE LOGIN {1}
    WITH PASSWORD = '{2}';
END
GO
USE [{0}];
    CREATE USER {1} FOR LOGIN {1}
    WITH DEFAULT_SCHEMA = dbo;
GO
