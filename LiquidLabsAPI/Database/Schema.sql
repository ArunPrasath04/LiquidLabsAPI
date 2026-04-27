IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'CryptoCoinsDB')
BEGIN
    CREATE DATABASE CryptoCoinsDB;
END
GO

USE CryptoCoinsDB;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Coins]') AND type in (N'U'))
                                BEGIN
                                    CREATE TABLE Coins (
                                        Id NVARCHAR(100) PRIMARY KEY,
                                        Symbol NVARCHAR(20),
                                        Name NVARCHAR(100),
                                        CurrentPrice DECIMAL(18, 8),
                                        MarketCap BIGINT,
                                        MarketCapRank INT,
                                        FullyDilutedValuation BIGINT,
                                        TotalVolume BIGINT,
                                        LastUpdated DATETIME
                                    )
                                END
GO