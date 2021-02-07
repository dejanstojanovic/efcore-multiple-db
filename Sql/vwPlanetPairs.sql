USE [Database1]
GO

CREATE OR ALTER VIEW [dbo].[vwPlanetPairs]
AS
	SELECT
	p1.Id, 
	p1.Name,
	CONCAT(p1.Name, ' - ', p2.Name) as Pair
	FROM Database2.dbo.Planets p1
	join Database2.dbo.Planets p2 on p1.Id!=p2.Id
GO
