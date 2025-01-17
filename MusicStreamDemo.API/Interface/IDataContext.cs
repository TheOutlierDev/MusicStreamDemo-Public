using MusicStreamDemo.Common;
using System.Data;

namespace MusicStreamDemo.API.Interface
{
    public interface IDataContext
    {
        Task<int> AddMusicAsync(Music music);
        IDbConnection CreateConnection();
        Task DeleteMusicAsync(int id);
        Task<IEnumerable<Music>> GetAllMusicAsync();
        Task<Music> GetMusicByIdAsync(int id);
    }
}