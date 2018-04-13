using System.Threading.Tasks;

namespace Data.DBInteractions
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDBFactory _databaseFactory;
        private DatabaseContext _dataContext;

        public UnitOfWork(IDBFactory databaseFactory)
        {
            this._databaseFactory = databaseFactory;
        }

        protected DatabaseContext DataContext
        {
            get { return _dataContext ?? (_dataContext = _databaseFactory.Get()); }
        }

        public bool Commit()
        {
            return DataContext.Commit();
        }

        public async Task<bool> CommitAsync()
        {
            return await DataContext.CommitAsync();
        }
    }
}
