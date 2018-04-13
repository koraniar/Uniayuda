using System;

namespace Data.DBInteractions
{
    public interface IDBFactory : IDisposable
    {
        DatabaseContext Get();
    }
}
