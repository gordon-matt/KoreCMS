ALTER TABLE Kore_Plugins_SimpleCommerce_Categories
ADD [Order] int NOT NULL DEFAULT(0)
GO

ALTER TABLE Kore_Pages
ADD [ShowOnMenus] bit NOT NULL DEFAULT(1)
GO

ALTER TABLE Kore_HistoricPages
ADD [ShowOnMenus] bit NOT NULL DEFAULT(1)
GO