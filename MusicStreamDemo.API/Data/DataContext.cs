using Dapper;
using MusicStreamDemo.API.Interface;
using MusicStreamDemo.Common;
using Npgsql;
using System.Data;

namespace MusicStreamDemo.API.Data
{
    public class DataContext : IDataContext
    {
        private readonly string _connectionString;

        public DataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<Music>> GetAllMusicAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Music>("SELECT * FROM Music ORDER BY UploadDate DESC");
        }

        public async Task<Music> GetMusicByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Music>("SELECT * FROM Music WHERE Id = @Id", new { Id = id });
        }

        public async Task<int> AddMusicAsync(Music music)
        {
            const string query = @"
                INSERT INTO Music (FileName, FileUrl, Title, Artist, Album, Duration, Tags)
                VALUES (@FileName, @FileUrl, @Title, @Artist, @Album, @Duration, @Tags)
                RETURNING Id;
            ";

            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, music);
        }

        public async Task DeleteMusicAsync(int id)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("DELETE FROM Music WHERE Id = @Id", new { Id = id });
        }
    }
}
