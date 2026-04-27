using LiquidLabsAPI.Models;
using Microsoft.Data.SqlClient;

namespace LiquidLabsAPI.DataAccess
{
    public class CryptoCoinRepository : ICryptoCoinRepository
    {
        private readonly string _connectionString;

        public CryptoCoinRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<List<CryptoCoin>> GetLocalCoinsAsync()
        {
            var coins = new List<CryptoCoin>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT Id, Symbol, Name, CurrentPrice, MarketCap, MarketCapRank, FullyDilutedValuation, TotalVolume, LastUpdated FROM Coins", conn);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                coins.Add(MapReaderToCoin(reader));
            }

            return coins;
        }

        public async Task<CryptoCoin> GetLocalCoinByIdAsync(string id)
        {
            var coin = new CryptoCoin();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT Id, Symbol, Name, CurrentPrice, MarketCap, MarketCapRank, FullyDilutedValuation, TotalVolume, LastUpdated FROM Coins WHERE Id = @Id", conn);

            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                coin = MapReaderToCoin(reader);
            }

            return coin;
        }

        public async Task<bool> CheckCoinsExistsAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            const string sql = @"
                                IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Coins]') AND type in (N'U'))
                                BEGIN
                                    SELECT CASE WHEN EXISTS (SELECT 1 FROM [dbo].[Coins]) THEN 1 ELSE 0 END
                                END
                                ELSE
                                BEGIN
                                    SELECT 0
                                END";

            using var cmd = new SqlCommand(sql, conn);
            await conn.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();

            return (int)(result ?? 0) == 1;
        }

        public async Task AddCoinsAsync(List<CryptoCoin> coins)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                var createTableSql = @"
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
                                END";

                using (var checkCmd = new SqlCommand(createTableSql, conn))
                {
                    await checkCmd.ExecuteNonQueryAsync();
                }

                using var transaction = conn.BeginTransaction();
                foreach (var coin in coins)
                {
                    var cmd = new SqlCommand(@"
                                        IF NOT EXISTS (SELECT 1 FROM Coins WHERE Id = @Id)
                                        INSERT INTO Coins (Id, Symbol, Name, CurrentPrice, MarketCap, MarketCapRank, FullyDilutedValuation, TotalVolume, LastUpdated)
                                        VALUES (@Id, @Symbol, @Name, @Price, @Cap, @CapRank, @FullyDVal, @TotVolume, @LastUpdated)", conn, transaction);

                    cmd.Parameters.AddWithValue("@Id", coin.Id);
                    cmd.Parameters.AddWithValue("@Symbol", coin.Symbol);
                    cmd.Parameters.AddWithValue("@Name", coin.Name);
                    cmd.Parameters.AddWithValue("@Price", coin.CurrentPrice);
                    cmd.Parameters.AddWithValue("@Cap", coin.MarketCap);
                    cmd.Parameters.AddWithValue("@CapRank", coin.MarketCapRank);
                    cmd.Parameters.AddWithValue("@FullyDVal", coin.FullyDilutedValuation);
                    cmd.Parameters.AddWithValue("@TotVolume", coin.TotalVolume);
                    cmd.Parameters.AddWithValue("@LastUpdated", coin.LastUpdated < new DateTime(1753, 1, 1) ? new DateTime(1753, 1, 1) : coin.LastUpdated);

                    await cmd.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        private CryptoCoin MapReaderToCoin(SqlDataReader reader) => new CryptoCoin
        {
            Id = reader["Id"].ToString(),
            Symbol = reader["Symbol"].ToString(),
            Name = reader["Name"].ToString(),
            CurrentPrice = (decimal)reader["CurrentPrice"],
            MarketCap = (long)reader["MarketCap"],
            MarketCapRank = (int)reader["MarketCapRank"],
            FullyDilutedValuation = (Int64)reader["FullyDilutedValuation"],
            TotalVolume = (Int64)reader["TotalVolume"],
            LastUpdated = (DateTime)reader["LastUpdated"]
        };
    }
}
