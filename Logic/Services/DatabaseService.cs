using Data.DBInteractions;
using Logic.Interfaces;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DatabaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool Commit()
        {
            return _unitOfWork.Commit();
        }

        public async Task<bool> CommitAsync()
        {
            return await _unitOfWork.CommitAsync();
        }
    }
}
