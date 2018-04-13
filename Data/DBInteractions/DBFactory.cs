

namespace Data.DBInteractions
{
    public class DBFactory: Disposable, IDBFactory
    {
        private DatabaseContext dataContext;

        public DatabaseContext Get()
        {
            if (dataContext == null)
            {
                dataContext = new DatabaseContext();
            }
            return dataContext;
        }
        protected override void DisposeCore()
        {
            if (dataContext != null)
            {
                dataContext.Dispose();
            }
        }
    }
}
