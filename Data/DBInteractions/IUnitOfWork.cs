using System.Threading.Tasks;

namespace Data.DBInteractions
{
    public interface IUnitOfWork
    {
        bool Commit();
        Task<bool> CommitAsync();
    }
}
