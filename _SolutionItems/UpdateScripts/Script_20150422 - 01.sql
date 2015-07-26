ALTER TABLE Kore_Pages
ADD ParentId uniqueidentifier NULL
GO
ALTER TABLE Kore_Pages
ADD [Order] int NOT NULL DEFAULT(0)
GO
ALTER TABLE Kore_HistoricPages
ADD ParentId uniqueidentifier NULL
GO
ALTER TABLE Kore_HistoricPages
ADD [Order] int NOT NULL DEFAULT(0)
GO