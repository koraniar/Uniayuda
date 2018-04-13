using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IDatabaseService
    {
        bool Commit();
        Task<bool> CommitAsync();
    }
}
